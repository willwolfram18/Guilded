import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { MaterialModule } from "@angular/material";
import { UniversalModule } from "angular2-universal";

import { AccountRoutingModule } from "./routing";

import { RegisterComponent } from "../components/register";
import { SignInComponent } from "../components/sign-in";

@NgModule({
    imports: [
        UniversalModule,
        MaterialModule.forRoot(),
        AccountRoutingModule,
        FormsModule,
    ],
    declarations: [
        RegisterComponent,
        SignInComponent,
    ],
})
export class AccountModule
{
}