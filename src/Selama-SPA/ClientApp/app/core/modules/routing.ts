import { NgModule } from "@angular/core";
import { RouterModule, Route } from "@angular/router";

import { HomeComponent } from "../../root/components/home";
import { CounterComponent } from "../../root/components/counter";
import { HomeComponent as ForumsHomeComponent } from "../../forums/components/home";

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
    {
        path: "forums",
        children: [
            {
                path: "",
                component: ForumsHomeComponent,
            },
            {
                path: "**",
                redirectTo: "",
                pathMatch: "full",
            },
        ],
    },
];

@NgModule({
    imports: [
        RouterModule.forRoot(ROUTES),
    ],
    exports: [
        RouterModule,
    ]
})
export class RoutingModule
{
}