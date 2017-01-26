import { NgModule } from "@angular/core"
import { RouterModule } from "@angular/router"

import { BaseModule } from "../../../core/modules/base"

import { AppComponent } from "../components/app"
import { RoutingModule } from "./routing"

@NgModule({
    imports: [
        BaseModule,
        RoutingModule,
    ],
    declarations: [
        AppComponent,
    ],
    bootstrap: [
        AppComponent
    ],
})
export class AppModule
{
}