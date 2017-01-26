import { NgModule } from "@angular/core"
import { Route, RouterModule } from "@angular/router"

import { AppComponent } from "../components/app"

const ROUTES: Route[] = [
    {
        path: "",
        component: AppComponent,
    },
    {
        path: "home",
        component: AppComponent,
    },
];

@NgModule({
    imports: [
        RouterModule.forRoot(ROUTES),
    ],
    exports: [
        RoutingModule,
    ]
})
export class RoutingModule
{
}