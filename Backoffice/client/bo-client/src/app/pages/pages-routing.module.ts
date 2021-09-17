import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from "./home/home.component";
import {SettingsComponent} from "./settings/settings.component";

const routes: Routes = [{
  path: "",
  component: HomeComponent,
  children: [
    {
      path: "settings",
      component: SettingsComponent
    },
    {
      path: "*",
      redirectTo: "",
    }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule {
}

export const routedComponents = [
  HomeComponent,
  SettingsComponent
];
