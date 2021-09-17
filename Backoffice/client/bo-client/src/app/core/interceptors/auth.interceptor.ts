import {Injectable} from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AuthStore} from "../stores/auth.store";
import {Router} from '@angular/router';
import {catchError} from 'rxjs/operators';
import {throwError} from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authStore: AuthStore,
              private router: Router) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authStore.getAuthToken();

    if (token && !request.url.includes(".json")) {
      request = request.clone({
        headers: request.headers.set('Authorization', 'Bearer ' + token)
      });
    }

    return next.handle(request).pipe(catchError((evt) => {
      if (evt instanceof HttpErrorResponse) {
        console.log(evt);
        if (evt.status === 401) {
          console.error("Not authenticated");
          this.router.navigate(['auth']);
        }
      }
      return throwError(evt);
    }));
  }
}
