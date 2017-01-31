import { NgModule } from "@angular/core";
import { UniversalModule } from "angular2-universal";

import { RoutingModule } from "./routing";

import { HomeComponent } from "../components/home";

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