import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {map, tap} from 'rxjs/operators';
import {Observable} from "rxjs";
import {of} from 'rxjs/internal/observable/of';
import {DateRenderComponent} from "../../../shared/dynamic-table/date-render.component";
import {FormBuilder} from "@angular/forms";
import {TableFieldType} from "../../../api/models";

@Injectable({
  providedIn: 'root'
})
export class AppService {

  constructor(protected http: HttpClient,
              protected fb: FormBuilder) {
  }

  getTableDataRequest(table: string) {
    // return this.http.get(`/api/${table}/list`);
    return this.getUserMockTableDataRequest();
  }

  getTableSettingsRequest(tableName: string) {
    return this.getUserMockTableSettingsRequest();
  }

  getFormFieldsRequest(table: string) {
    return this.getUserMockFormFieldsRequest();
  }


  getUserMockTableDataRequest() {
    return of([{
      createAt: "2021-09-16T17:03:01.926Z",
      emailAddress: "lielyehosh@gmail.com",
      id: "614378c582129ce1855d8e63",
      idNumber: "1234",
      modifyAt: "2021-09-16T17:03:01.926Z",
      username: "Liel Yehoshua"
    }, {
      createAt: "2021-09-16T17:03:01.926Z",
      emailAddress: "sam@gmail.com",
      id: "614378c582129ce1855d8e63",
      idNumber: "1234",
      modifyAt: "2021-09-16T17:03:01.926Z",
      username: "Sam"
    }]);
  }

  // getUserMockTableColumnsRequest() : Array<TableColumn> {
  //   return [
  //     {
  //       id: 'createAt',
  //       title: 'Create At',
  //       addable: false,
  //       editable: false,
  //       type: TableColumnType.DATE
  //     },
  //     {
  //       id: 'modifyAt',
  //       title: 'Modify At',
  //       addable: false,
  //       editable: false,
  //       type: TableColumnType.DATE
  //     },
  //     {
  //       id: 'username',
  //       title: 'Username',
  //       addable: true,
  //       editable: true,
  //       type: TableColumnType.TEXT
  //     },
  //     {
  //       id: 'idNumber',
  //       title: 'Id Number',
  //       addable: true,
  //       editable: true,
  //       type: TableColumnType.TEXT
  //     },
  //     {
  //       id: 'emailAddress',
  //       title: 'Email',
  //       addable: true,
  //       editable: true,
  //       type: TableColumnType.TEXT
  //     }
  //   ];
  // }


  getUserMockTableSettingsRequest() {
    return of({
      columns: {
        createAt: {
          title: 'Create At',
          addable: false,
          editable: false,
          ...this.GetCustomRenderComponentByType(TableColumnType.DATE)
        },
        modifyAt: {
          title: 'Modify At',
          addable: false,
          editable: false,
          ...this.GetCustomRenderComponentByType(TableColumnType.DATE)
        },
        username: {
          title: 'Username',
        },
        idNumber: {
          title: 'Id Number'
        },
        emailAddress: {
          title: 'Email Address'
        }
      },
      mode: 'external',
      edit: {
        editButtonContent: `edit`,
        inputClass: 'table-action'
      },
      actions: {
        add: false,
        edit: false,
        delete: false
      }
    });
  }

  private GetCustomRenderComponentByType(type: TableColumnType) {
    switch (type) {
      case TableColumnType.DATE:
        return {
          type: 'custom',
          renderComponent: DateRenderComponent
        };
      default:
        return {
          type: 'text'
        };
    }
  }

  private getUserMockFormFieldsRequest() {
    return [
      {
        type: TableFieldType.String,
        control: this.fb.control({value: "", disabled: true}),
        label: "Email Adddress",
        name: 'email'
      },
      {
        type: TableFieldType.String,
        control: this.fb.control({value: "", disabled: true}),
        label: "Username",
        name: 'username'
      },
      {
        type: TableFieldType.String,
        control: this.fb.control({value: "", disabled: true}),
        label: "Id Number",
        name: 'idNumber'
      }
    ];
  }
}

enum TableColumnType {
  TEXT = 'text',
  DATE = 'date'
}
