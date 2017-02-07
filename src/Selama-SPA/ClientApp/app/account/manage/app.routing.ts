import { NgModule } from "@angular/core";
import { RouterModule, Route } from "@angular/router";

import { ManagerGuard } from "./manager-guard.service";
import { HomeComponent } from "./home.component";

const ROUTES: Route[] = [
    {
        path: "account",
        children: [
            {
                path: "manage",
                canActivateChild: [
                    ManagerGuard,
                ],
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
                ],
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
    providers: [
        ManagerGuard,
    ],
})
export class RoutingModule
{
}