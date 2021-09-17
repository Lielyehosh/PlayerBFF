import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NbThemeModule, NbLayoutModule, NbSidebarModule, NbButtonModule, NbMenuService, NbMenuModule} from '@nebular/theme';
import {NbEvaIconsModule} from '@nebular/eva-icons';
import {ThemeModule} from "./theme/theme.module";
import {HomeComponent} from './home/home.component';
import {RouterModule} from "@angular/router";
import {HttpClientModule} from "@angular/common/http";
import {NbAuthJWTToken, NbAuthModule, NbPasswordAuthStrategy} from "@nebular/auth";

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
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
        }),
      ],
      forms: {
      },
    }),
    HttpClientModule,
    NbMenuModule.forRoot()
  ],
  providers: [NbMenuService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
