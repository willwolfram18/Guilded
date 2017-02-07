import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AuthService, SignInModel } from "../core/auth.service";
import { ProgressBarService } from "../core/progress-bar.service";

@Component({
    selector: "account-sign-in",
    template: require("./templates/sign-in.html"),
    styles: [
        require("./templates/sign-in.css"),
    ],
})
export class SignInComponent
{
    user: SignInModel = new SignInModel();
    isLoading: boolean = false;
    formErrors: SignInErrors = new SignInErrors();

    constructor(private authService: AuthService, private router: Router, private progressBar: ProgressBarService)
    {
    }

    public logIn(): void
    {
        if (!this.isLoading)
        {
            this.isLoading = true;
            this.progressBar.start();
            this.authService.logIn(this.user)
                .finally(() => 
                {
                    this.isLoading = false;
                    this.progressBar.complete();
                })
                .subscribe(
                    result => this.router.navigate(["/home"]),
                    errors => this.parseErrors(errors.json()),
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

class SignInErrors
{
    Model: string[] = [];
    Username: string[] = [];
    Password: string[] = [];
}