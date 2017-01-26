import { Component } from "@angular/core"
import { Route } from "@angular/router"

@Component({
    selector: "ng-menu",
    templateUrl: "templates/menu"
})
export class MenuComponent 
{
    static MenuRoute: Route = {
        path: "",
        component: MenuComponent,
        outlet: "menu",
    };
}