import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {
  NbThemeModule,
  NbLayoutModule,
  NbSidebarModule,
  NbButtonModule,
  NbMenuService,
  NbMenuModule, NbDatepickerModule
} from '@nebular/theme';
import {NbEvaIconsModule} from '@nebular/eva-icons';
import {ThemeModule} from "./theme/theme.module";
import {RouterModule} from "@angular/router";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {NbAuthJWTToken, NbAuthModule, NbPasswordAuthStrategy} from "@nebular/auth";
import {AuthInterceptor} from "./core/interceptors/auth.interceptor";
import {DynamicFormModule} from "./shared/dynamic-form/dynamic-form.module";
import {Ng2SmartTableModule} from "ng2-smart-table";
import {DynamicTableModule} from "./shared/dynamic-table/dynamic-table.module";
import {ApiModule} from "./api/api.module";
import {environment} from "../environments/environment";

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    NbThemeModule.forRoot({name: 'dark'}),
    NbLayoutModule,
    NbEvaIconsModule,
    ThemeModule.forRoot(),
    NbSidebarModule.forRoot(),
    RouterModule,
    NbButtonModule,
    NbAuthModule.forRoot({
      strategies: [
        NbPasswordAuthStrategy.setup({
          name: 'email',
          baseEndpoint: '/api/auth/',
          token: {
            class: NbAuthJWTToken,
            key: 'token', // this parameter tells where to look for the token
          },
          login: {
            alwaysFail: false,
            endpoint: 'login',
            method: 'post',
            requireValidToken: true,
            redirect: {
              success: '/home',
              failure: null,
            },
            defaultErrors: ['Login/Email combination is not correct, please try again.'],
            defaultMessages: ['You have been successfully logged in.'],
          },
          register: {
            endpoint: 'register',
            method: 'post'
          },
          resetPass: {
            endpoint: 'reset-pass',
            method: 'post'
          }
        }),
      ],
      forms: {},
    }),
    HttpClientModule,
    NbMenuModule.forRoot(),
    DynamicFormModule,
    DynamicTableModule,
    Ng2SmartTableModule,
    NbDatepickerModule.forRoot(),
    ApiModule.forRoot({
      rootUrl: environment.baseUrl,
    }),
  ],
  providers: [
    NbMenuService,
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
