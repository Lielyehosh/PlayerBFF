import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {PagesRoutingModule, routedComponents} from './pages-routing.module';
import {SettingsComponent} from './settings/settings.component';
import {ThemeModule} from '../theme/theme.module';
import {
    NbAccordionModule, NbActionsModule,
    NbButtonModule,
    NbCardModule,
    NbIconModule,
    NbInputModule,
    NbLayoutModule,
    NbListModule, NbTabsetModule
} from "@nebular/theme";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {DynamicFormModule} from "../shared/dynamic-form/dynamic-form.module";
import {GameComponent} from './game/game.component';
import {UserComponent} from './user/user.component';
import {Ng2SmartTableModule} from "ng2-smart-table";
import {DynamicTableModule} from "../shared/dynamic-table/dynamic-table.module";


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
        NbLayoutModule,
        Ng2SmartTableModule,
        DynamicTableModule,
        NbTabsetModule,
        NbActionsModule,
    ],
  declarations: [...routedComponents, SettingsComponent, GameComponent, UserComponent]
})
export class PagesModule {
}
