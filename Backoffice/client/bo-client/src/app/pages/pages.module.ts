import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {PagesRoutingModule, routedComponents} from './pages-routing.module';
import {SettingsComponent} from './settings/settings.component';
import {ThemeModule} from '../theme/theme.module';
import {
    NbAccordionModule,
    NbButtonModule,
    NbCardModule,
    NbIconModule,
    NbInputModule,
    NbLayoutModule,
    NbListModule
} from "@nebular/theme";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {DynamicFormModule} from "../shared/dynamic-form/dynamic-form.module";
import { GameComponent } from './game/game.component';


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
        PagesRoutingModule,
        ReactiveFormsModule,
        DynamicFormModule,
        NbLayoutModule
    ],
  declarations: [...routedComponents, SettingsComponent, GameComponent]
})
export class PagesModule {
}
