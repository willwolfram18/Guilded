import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AuthService } from "../../core/services/auth";
import { ProgressBarService } from "../../core/services/progress-bar";

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
    isLoading: boolean = false;
    formErrors: FormErrors = new FormErrors();

    constructor(private authService: AuthService, private router: Router, private progressBar: ProgressBarService)
    {
    }

    public logIn(): void
    {
        if (!this.isLoading)
        {
            this.isLoading = true;
            this.progressBar.start();
            this.authService.logIn(this.user.Email, this.user.Password, this.user.RememberMe)
                .finally(() => 
                {
                    this.isLoading = false;
                    this.progressBar.complete();
                })
                .subscribe(
                    result => this.router.navigate(["/home"]),
                    result => this.parseErrors(result.json()),
                );
        }
    }
    private parseErrors(errors: any): void
    {
        this.formErrors.Model = errors[""] || [];
        this.formErrors.Username = errors.Username || [];
        this.formErrors.Password = errors.Password || [];
    }
}

class SignInModel
{
    Email: string;
    Password: string;
    RememberMe: boolean = false;
}
class FormErrors
{
    Model: string[] = [];
    Username: string[] = [];
    Password: string[] = [];
}