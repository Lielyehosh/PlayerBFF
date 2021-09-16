import {OnDestroy} from '@angular/core';
import {Component, OnInit} from '@angular/core';
import {Observable} from 'rxjs';
import {Subject} from 'rxjs';
import {BehaviorSubject} from 'rxjs';
import {User, UserData} from "../../../core/data/user";
import {NbMenuItem, NbMenuService, NbSidebarService} from "@nebular/theme";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  private destroy$: Subject<void> = new Subject<void>();
  user?: Observable<User>;
  title?: Observable<string>;

  constructor(private sidebarService: NbSidebarService,
              private menuService: NbMenuService) {
  }

  ngOnInit() {
    this.user = user.asObservable();
    this.title = title.asObservable();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  navigateHome() {
    this.menuService.navigateHome();
    return false;
  }
}

const user = new BehaviorSubject<User>({});
export function setHeaderUser(_user: User) {
  user.next(_user);
}

const title = new BehaviorSubject<string>('Title');
export function setHeaderTitle(_title: string) {
  title.next(_title);
}
