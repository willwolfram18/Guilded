import { NgModule } from "@angular/core";
import { RouterModule, Route } from "@angular/router";
import { UniversalModule } from "angular2-universal";

import { SignInGuard } from "./sign-in-guard.service";
import { RegisterComponent } from "./register.component";
import { SignInComponent } from "./sign-in.component";

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
                canActivate: [
                    SignInGuard,
                ],
            },
            {
                path: "register",
                component: RegisterComponent,
                canActivate: [
                    SignInGuard,
                ],
            },
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