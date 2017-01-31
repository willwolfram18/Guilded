import { NgModule } from "@angular/core";
import { Route, RouterModule } from "@angular/router";

import { HomeComponent } from "../components/home";

const ROUTES: Route[] = [
    {
        path: "forums",
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
                path: "**",
                redirectTo: "",
                pathMatch: "full",
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
    ]
})
export class RoutingModule
{
}