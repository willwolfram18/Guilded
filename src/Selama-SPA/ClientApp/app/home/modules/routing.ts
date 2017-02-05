import { NgModule } from "@angular/core";
import { Route, RouterModule } from "@angular/router";

import { CounterComponent } from "../components/counter";
import { HomeComponent } from "../components/home";

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