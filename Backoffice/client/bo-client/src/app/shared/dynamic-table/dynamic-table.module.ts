import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {DynamicTableComponent} from "./dynamic-table.component";
import {Ng2SmartTableModule} from "ng2-smart-table";
import {DateRenderComponent} from "./date-render.component";


@NgModule({
  declarations: [
    DynamicTableComponent,
    DateRenderComponent,
  ],
  imports: [
    CommonModule,
    Ng2SmartTableModule
  ],
  exports: [DynamicTableComponent]
})
export class DynamicTableModule {
}
