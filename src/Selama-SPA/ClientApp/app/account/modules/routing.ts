import { NgModule } from "@angular/core";
import { RouterModule, Route } from "@angular/router";
import { UniversalModule } from "angular2-universal";

import { SignInComponent } from "../components/sign-in";
import { RegisterComponent } from "../components/register";

const ROUTES: Route[] = [
    {
        path: "account",
        children: [
            {
                path: "",
                redirectTo: "sign-in",
                pathMatch: "full",
            },
            {
                path: "sign-in",
                component: SignInComponent,
            },
            {
                path: "register",
                component: RegisterComponent
            },
            {
                path: "**",
                redirectTo: "sign-in",
                pathMatch: "full",
            }
        ]
    }
];

@NgModule({
    imports: [
        UniversalModule,
        RouterModule.forChild(ROUTES),
    ],
    exports: [
        RouterModule,
    ],
})
export class AccountRoutingModule
{
}