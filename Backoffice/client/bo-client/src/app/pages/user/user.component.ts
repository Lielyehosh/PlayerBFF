import {Component, OnInit} from '@angular/core';
import {Observable} from "rxjs";
import {UserService} from "../../api/services/user.service";
import {UserView} from "../../api/models/user-view";
import {TableForm} from "../../api/models/table-form";
import {catchError} from "rxjs/operators";

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  userFormScheme?: TableForm;
  userListObs?: Observable<Array<UserView>>;
  isLoading: boolean = true;

  constructor(protected userService: UserService) {
  }

  ngOnInit(): void {
    this.userListObs = this.userService.apiUserListGet$Json();
    this.userService.apiUserFormGet$Json()
      .subscribe(
        res => {
          this.userFormScheme = res;
          this.isLoading = false;
          },
          err => catchError(err)
      );
  }

  onSubmit($event: any) {
    console.log($event);
  }
}
