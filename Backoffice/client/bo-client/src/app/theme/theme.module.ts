import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from "@angular/router";
import {NbButtonModule, NbSidebarModule} from '@nebular/theme';
import {NbLayoutModule} from "@nebular/theme";
import { EmptyLayoutPageComponent } from './layouts/empty-layout-page/empty-layout-page.component';
import { ModuleWithProviders } from '@angular/core';
import { HeaderComponent } from './components/header/header.component';
import {NbAuthModule} from "@nebular/auth";


const COMPONENTS = [
  EmptyLayoutPageComponent,
  HeaderComponent
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    NbLayoutModule,
    NbSidebarModule,
    NbButtonModule,
    NbAuthModule
  ],
  declarations: [
    ...COMPONENTS,
    HeaderComponent
  ],
  exports: [
    CommonModule,
    ...COMPONENTS
  ]
})
export class ThemeModule {
  static forRoot(): ModuleWithProviders<ThemeModule> {
    return {
      ngModule: ThemeModule,
    };
  }
}
