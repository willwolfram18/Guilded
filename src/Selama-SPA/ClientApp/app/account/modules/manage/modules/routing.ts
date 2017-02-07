import { NgModule } from "@angular/core";
import { RouterModule, Route } from "@angular/router";

import { ManagerGuard } from "../components/manager.guard";
import { HomeComponent } from "../components/home";

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