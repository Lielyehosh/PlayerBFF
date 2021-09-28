import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {DynamicFormComponent} from "./dynamic-form.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {
  NbAccordionModule,
  NbButtonModule,
  NbCardModule, NbCheckboxModule, NbDatepickerModule,
  NbIconModule,
  NbInputModule,
  NbListModule
} from "@nebular/theme";
import {ThemeModule} from "../../theme/theme.module";
import {PagesRoutingModule} from "../../pages/pages-routing.module";

@NgModule({
  declarations: [
    DynamicFormComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NbCardModule,
    ThemeModule,
    NbIconModule,
    NbAccordionModule,
    NbButtonModule,
    FormsModule,
    NbListModule,
    NbInputModule,
    NbCardModule,
    PagesRoutingModule,
    ReactiveFormsModule,
    NbCheckboxModule,
    NbDatepickerModule,
  ],
  exports: [
    DynamicFormComponent
  ]
})
export class DynamicFormModule {
}
