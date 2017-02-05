import { Component } from "@angular/core";

import { AuthService } from "../../core/services/auth";

@Component({
    selector: "account-register",
    template: require("../templates/register.html"),
    styles: [
        require("../templates/register.css"),
    ],
})
export class RegisterComponent
{
    modelErrors: string[] = [];
    isLoading: boolean = false;
    user: RegisterUserModel = new RegisterUserModel();

    constructor(private authService: AuthService)
    {
    }

    public register(): void
    {
        console.log(this.user.ConfirmPassword, this.user.Email, this.user.Password, this.user.Username);
    }
}

class RegisterUserModel
{
    Username: string;
    Email: string;
    Password: string;
    ConfirmPassword: string;
}