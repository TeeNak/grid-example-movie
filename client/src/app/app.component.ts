import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GridOptions } from 'ag-grid/main';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/operator/map';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  gridOptions: GridOptions;
  columnDefs: any[];
  rowData: any[];

  public message;

  constructor(public http: HttpClient) {

    this.gridOptions = <GridOptions>{};

    this.columnDefs = [
      {headerName: 'Code', field: 'code', editable: true},
      {headerName: 'Name', field: 'name', editable: true},
      {headerName: 'Version', field: 'version'}
    ];

    this.rowData = [
    ];
  }

  onGridReady(params) {
    params.api.sizeColumnsToFit();
  }

  public doLoadData = function() {
    const response: Observable<Response> =
        this.http.get('/hrdemo/jobs?size=100000');

    const result = new Promise<{}>(
      (resolve: (value?: any) => void, reject: (reason?: any) => void) => {
        response
        .subscribe(
            (json: any) => {
                const data = json._embedded.jobs;
                this.rowData = data;
                resolve(data);
            },
            (err) => {
                this.message = err;
                reject(err);
            }
        );
    });

    return result;
  };

  public loadData($event) {
    this.doLoadData().then( () => {
        this.message = 'Successfully loaded the data.';
    });

  }

  public saveData($event) {
    const data: string = JSON.stringify(this.rowData);
    const options = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    const response: Observable<Object> =
        this.http.put('/hrdemo/jobs/list', data, options);
    response.subscribe( () => {
        // reload
        this.doLoadData().then( () => {
            this.message = 'Successfully saved and reloaded the data.';
        });

    });

  }

}
