import { ModuleWithProviders } from "@angular/core"
import { RouterModule, Route } from "@angular/router"

import { FooterComponent } from "../components/footer"
import { MenuComponent } from "../components/menu"

export class RoutingFactoryService
{
    static create(appRoutes: Route[]): ModuleWithProviders
    {
        appRoutes.push(FooterComponent.FooterRoute);
        appRoutes.push(MenuComponent.MenuRoute);

        return RouterModule.forRoot(appRoutes);
    }
}