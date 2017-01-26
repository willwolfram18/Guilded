import { NgModule } from "@angular/core"

import { BaseModule } from "../../../core/modules/base"
import { RoutingFactoryService } from "../../../core/services/routing-factory"

import { AppComponent } from "../components/app"
import { ROUTES } from "./routing"

@NgModule({
    imports: [
        BaseModule,
        RoutingFactoryService.create(ROUTES),
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