/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { TableForm } from '../models/table-form';
import { UserView } from '../models/user-view';

@Injectable({
  providedIn: 'root',
})
export class UserService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiUserListGet
   */
  static readonly ApiUserListGetPath = '/api/user/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiUserListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiUserListGet$Plain$Response(params?: {

  }): Observable<StrictHttpResponse<Array<UserView>>> {

    const rb = new RequestBuilder(this.rootUrl, UserService.ApiUserListGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<UserView>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiUserListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiUserListGet$Plain(params?: {

  }): Observable<Array<UserView>> {

    return this.apiUserListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<UserView>>) => r.body as Array<UserView>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiUserListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiUserListGet$Json$Response(params?: {

  }): Observable<StrictHttpResponse<Array<UserView>>> {

    const rb = new RequestBuilder(this.rootUrl, UserService.ApiUserListGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<UserView>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiUserListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiUserListGet$Json(params?: {

  }): Observable<Array<UserView>> {

    return this.apiUserListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<UserView>>) => r.body as Array<UserView>)
    );
  }

  /**
   * Path part for operation apiUserFormGet
   */
  static readonly ApiUserFormGetPath = '/api/user/form';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiUserFormGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiUserFormGet$Plain$Response(params?: {

  }): Observable<StrictHttpResponse<TableForm>> {

    const rb = new RequestBuilder(this.rootUrl, UserService.ApiUserFormGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<TableForm>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiUserFormGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiUserFormGet$Plain(params?: {

  }): Observable<TableForm> {

    return this.apiUserFormGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<TableForm>) => r.body as TableForm)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiUserFormGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiUserFormGet$Json$Response(params?: {

  }): Observable<StrictHttpResponse<TableForm>> {

    const rb = new RequestBuilder(this.rootUrl, UserService.ApiUserFormGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<TableForm>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiUserFormGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiUserFormGet$Json(params?: {

  }): Observable<TableForm> {

    return this.apiUserFormGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<TableForm>) => r.body as TableForm)
    );
  }

}
