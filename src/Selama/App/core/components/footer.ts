import { Component } from "@angular/core"
import { Route } from "@angular/router"

@Component({
    selector: "ng-footer",
    templateUrl: "templates/footer"
})
export class FooterComponent
{
    static FooterRoute: Route = {
        path: "",
        component: FooterComponent,
        outlet: "footer"
    };
}