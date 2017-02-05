import { NgModule } from "@angular/core";
import { UniversalModule } from "angular2-universal";

import { AccountRoutingModule } from "./routing";

import { RegisterComponent } from "../components/register";
import { SignInComponent } from "../components/sign-in";

@NgModule({
    imports: [
        UniversalModule,
        AccountRoutingModule,
    ],
    declarations: [
        RegisterComponent,
        SignInComponent,
    ],
})
export class AccountModule
{
}