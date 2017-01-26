import { Component } from "@angular/core"
import { Route } from "@angular/router"

@Component({
    templateUrl: "templates/footer"
})
export class FooterComponent
{
    static FooterRoute: Route = {
        path: "templates/footer",
        component: FooterComponent,
        outlet: "footer"
    };
}