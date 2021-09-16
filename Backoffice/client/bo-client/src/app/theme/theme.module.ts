import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from "@angular/router";
import {NbButtonModule, NbSidebarModule} from '@nebular/theme';
import {NbLayoutModule} from "@nebular/theme";
import { EmptyLayoutPageComponent } from './layouts/empty-layout-page/empty-layout-page.component';
import { ModuleWithProviders } from '@angular/core';


const COMPONENTS = [
  EmptyLayoutPageComponent
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    NbLayoutModule,
    NbSidebarModule,
    NbButtonModule,
  ],
  declarations: [
    ...COMPONENTS
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
