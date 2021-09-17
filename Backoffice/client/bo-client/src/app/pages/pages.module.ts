import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {PagesRoutingModule, routedComponents} from './pages-routing.module';
import {SettingsComponent} from './settings/settings.component';
import {ThemeModule} from '../theme/theme.module';
import {NbAccordionModule, NbButtonModule, NbCardModule, NbIconModule, NbInputModule, NbListModule} from "@nebular/theme";
import {FormsModule} from "@angular/forms";


@NgModule({
  imports: [
    CommonModule,
    ThemeModule,
    NbIconModule,
    NbAccordionModule,
    NbButtonModule,
    FormsModule,
    NbListModule,
    NbInputModule,
    NbCardModule,
    PagesRoutingModule
  ],
  declarations: [...routedComponents, SettingsComponent]
})
export class PagesModule {
}
