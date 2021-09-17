import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  Router,
  RouterStateSnapshot,
  UrlTree
} from '@angular/router';
import { NbAuthService } from '@nebular/auth';
import { NbMenuService } from '@nebular/theme';
import { Observable } from 'rxjs';
import {AuthStore} from "../stores/auth.store";
import {filter} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authStore: AuthStore,
    private nbMenuService: NbMenuService,
  ) {
    this.nbMenuService.onItemClick()
      .pipe(filter((menuBag) => menuBag.item.title === 'Logout'))
      .subscribe((menuBag) => {
        this.authStore.setAuthToken(null, {});
        this.router.navigateByUrl(`/auth`);
    });
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot):
    Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (!this.authStore.getAuthToken()) {
      return this.router.parseUrl(`auth/login`);
    }
    return true;
  }

  // canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
  //   if (!this.authService.getToken()) {
  //     return this.router.parseUrl(`/auth/login`);
  //   }
  //   return true;
  // }
}
