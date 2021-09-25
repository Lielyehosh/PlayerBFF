import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  settings = {
    columns: {
      createAt: {
        title: 'Create At'
      },
      modifyAt: {
        title: 'Modify At'
      },
      username: {
        title: 'Username'
      },
      idNumber: {
        title: 'Id Number'
      },
      emailAddress: {
        title: 'Email Address'
      }
    }
  };
  data = null;
  dataObs?: Observable<Object>;

  constructor(protected http: HttpClient) { }

  ngOnInit(): void {
    this.dataObs = this.http.get(`/api/user/list`);
  }

  onCreate($event: any) {

  }
  onCreateConfirm($event: any) {

  }
}
