import { NgModule } from "@angular/core";
import { UniversalModule } from "angular2-universal";

import { RoutingModule } from "./app.routing";

import { HomeComponent } from "./home.component";

@NgModule({
    imports: [
        UniversalModule,
        RoutingModule,
    ],
    declarations: [
        HomeComponent,
    ]
})
export class ForumsAppModule
{
}