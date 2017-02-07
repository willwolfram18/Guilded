import { NgModule } from "@angular/core";
import { Route, RouterModule } from "@angular/router";

import { CounterComponent } from "./counter.component";
import { HomeComponent } from "./home.component";

const ROUTES: Route[] = [
    {
        path: "",
        children: [
            {
                path: "",
                redirectTo: "home",
                pathMatch: "full",
            },
            {
                path: "home",
                component: HomeComponent,
            },
            {
                path: "counter",
                component: CounterComponent,
            },
        ],
    },
];

@NgModule({
    imports: [
        RouterModule.forChild(ROUTES),
    ],
    exports: [
        RouterModule,
    ],
})
export class RoutingModule
{
}