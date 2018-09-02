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

  public message: string;
  public isError: boolean;

  constructor(public http: HttpClient) {

    this.gridOptions = <GridOptions>{};

    this.columnDefs = [
      {headerName: 'Code', field: 'Code', editable: true},
      {headerName: 'Name', field: 'Name', editable: true},
      {headerName: 'Version', field: 'Version'}
    ];

    this.rowData = [
    ];
  }

  onGridReady(params) {
    params.api.sizeColumnsToFit();
  }

  public doLoadData = function() {
    const response: Observable<Response> =
        this.http.get('/Movies');

    const result = new Promise<{}>(
      (resolve: (value?: any) => void, reject: (reason?: any) => void) => {
        response
        .subscribe(
            (json: any) => {
                const data = json.value;
                this.rowData = data;
                resolve(data);
            },
            (err) => {
                this.message = err;
                this.isError = true;
                reject(err);
            }
        );
    });

    return result;
  };

  public loadData($event) {
    this.doLoadData().then( () => {
        this.message = 'Successfully loaded the data.';
        this.isError = false;
    });

  }

  public saveData($event) {
    const data: string = JSON.stringify({ value: this.rowData });
    const options = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    const response: Observable<Object> =
        this.http.post('/Movies/Action.UpdateAll/', data, options);
    response.subscribe(
        () => {
            // reload
            this.doLoadData().then( () => {
                this.message = 'Successfully saved and reloaded the data.';
                this.isError = false;
            });
            this.isError = false;
        },
        (err) => {
            const message = err && err.error && err.error.error && err.error.error.message || 'error!';
            this.message = message;
            this.isError = true;
        }
    );

  }

}
