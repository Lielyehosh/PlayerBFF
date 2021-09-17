import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from "@angular/router";
import {
  NbActionsModule,
  NbButtonModule,
  NbCardModule,
  NbContextMenuModule, NbMenuModule,
  NbSidebarModule,
  NbUserModule
} from '@nebular/theme';
import {NbLayoutModule} from "@nebular/theme";
import {ModuleWithProviders} from '@angular/core';
import {HeaderComponent} from './components/header/header.component';
import {NbAuthModule} from "@nebular/auth";
import {SideBarComponent} from './components/side-bar/side-bar.component';
import {DashboardLayoutComponent} from './layouts/dashboard-layout/dashboard-layout.component';


const COMPONENTS = [
  DashboardLayoutComponent,
  HeaderComponent,
  SideBarComponent
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    NbLayoutModule,
    NbSidebarModule,
    NbButtonModule,
    NbAuthModule,
    NbActionsModule,
    NbUserModule,
    NbContextMenuModule,
    NbCardModule,
    NbMenuModule
  ],
  declarations: [
    ...COMPONENTS,
  ],
  exports: [
    CommonModule,
    ...COMPONENTS,
  ]
})
export class ThemeModule {
  static forRoot(): ModuleWithProviders<ThemeModule> {
    return {
      ngModule: ThemeModule,
    };
  }
}
