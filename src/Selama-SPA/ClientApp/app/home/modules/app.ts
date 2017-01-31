import { NgModule } from "@angular/core";
import { UniversalModule } from "angular2-universal";

import { RoutingModule } from "./routing";

import { CounterComponent } from "../components/counter";
import { GuildActivityComponent } from "../components/guild-activity";
import { HomeComponent } from "../components/home";

@NgModule({
    imports: [
        UniversalModule,
        RoutingModule,
    ],
    declarations: [
        CounterComponent,
        GuildActivityComponent,
        HomeComponent,
    ],
})
export class HomeAppModule
{
}