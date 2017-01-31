import { NgModule } from "@angular/core";
import { MaterialModule } from "@angular/material";
import { UniversalModule } from "angular2-universal";

import { RoutingModule } from "./routing";

import { CounterComponent } from "../components/counter";
import { GuildActivityComponent } from "../components/guild-activity";
import { HomeComponent } from "../components/home";

@NgModule({
    imports: [
        UniversalModule,
        MaterialModule.forRoot(),
        RoutingModule,
    ],
    declarations: [
        CounterComponent,
        GuildActivityComponent,
        HomeComponent,
    ],
    exports: [
        MaterialModule,
    ],
})
export class HomeAppModule
{
}