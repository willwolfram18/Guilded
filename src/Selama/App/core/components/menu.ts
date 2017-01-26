import { Component } from "@angular/core"
import { Route } from "@angular/router"

@Component({
    templateUrl: "templates/menu"
})
export class MenuComponent
{
    static MenuRoute: Route = {
        path: "templates/menu",
        component: MenuComponent,
        outlet: "menu",
    };
}