import { Component } from "@angular/core";

@Component({
    selector: "account-sign-in",
    template: require("../templates/sign-in.html"),
    styles: [
        require("../templates/sign-in.css"),
    ],
})
export class SignInComponent
{
    user: SignInModel = new SignInModel();

    public log(): void
    {
        console.log(this.user.Email, this.user.Password, this.user.RememberMe);
    }
}

class SignInModel
{
    Email: string;
    Password: string;
    RememberMe: boolean = false;
}