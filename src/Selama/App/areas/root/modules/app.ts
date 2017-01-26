import { NgModule } from "@angular/core"

import { BaseModule } from "../../../core/modules/base"
import { RoutingFactoryService } from "../../../core/services/routing-factory"
import { FooterComponent } from "../../../core/components/footer"
import { MenuComponent } from "../../../core/components/menu"

import { AppComponent } from "../components/app"
import { HomeComponent } from "../components/home"
import { ROUTES } from "./routing"

@NgModule({
    imports: [
        BaseModule,
        RoutingFactoryService.create(ROUTES),
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        FooterComponent,
        MenuComponent,
    ],
    bootstrap: [
        AppComponent
    ],
})
export class AppModule
{
}