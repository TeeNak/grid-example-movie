using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Odata4AspNet.Models
{
    public class MoviesContextInitializer
        : DropCreateDatabaseIfModelChanges<MoviesContext>
    {

        public static DataTable ListToDataTable<T>(IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static string GetTableName(Type type, DbContext context)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                    .Single()
                    .EntitySetMappings
                    .Single(s => s.EntitySet == entitySet);

            // Find the storage entity set (table) that the entity is mapped
            var table = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            return (string)table.MetadataProperties["Table"].Value ?? table.Name;
        }


        private static void BulkWrite<T>(DbContext context, IEnumerable<T> entities)
        {
            var tableName = GetTableName(typeof(T), context);

            SqlConnection conn = context.Database.Connection as SqlConnection;
            conn.Open();

            context.Database.ExecuteSqlCommand($"TRUNCATE TABLE {tableName}");

            using (var bulkCopy = new SqlBulkCopy(conn))
            {

                var table = ListToDataTable(entities);

                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(table);

            }
        }


        protected override void Seed(Odata4AspNet.Models.MoviesContext context)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();

            string resourceName = "Odata4AspNet.Models.SeedData.Movies.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var csvReader = new CsvReader(reader);
                    csvReader.Configuration.RegisterClassMap<MovieCsvMapper>();
                    var movies = csvReader.GetRecords<Movie>().ToArray();

                    if (!(context.Database.Connection is SqlConnection))
                    {
                        context.Movies.AddOrUpdate(c => c.Id, movies);
                    }
                    else
                    {
                        BulkWrite(context, movies);
                    }

                }
            }

        }

    }
}