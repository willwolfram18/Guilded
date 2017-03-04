import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AuthService, RegisterUserModel } from "../core/auth.service";
import { ProgressBarService } from "../core/progress-bar.service";

@Component({
    selector: "account-register",
    template: require("./templates/register.html"),
    styles: [
        require("./templates/register.scss"),
    ],
})
export class RegisterComponent
{
    formErrors: RegisterErrors = new RegisterErrors();
    isLoading: boolean = false;
    user: RegisterUserModel = new RegisterUserModel();

    constructor(private authService: AuthService, private progressBar: ProgressBarService,
        private router: Router
    )
    {
    }

    public register(): void
    {
        if (!this.isLoading)
        {
            this.isLoading = true;
            this.progressBar.start();
            this.authService.register(this.user)
                .finally(() =>
                {
                    this.isLoading = false;
                    this.progressBar.complete();
                })
                .subscribe(
                    result => this.router.navigate(["/home"]),
                    errors => this.parseErrors(errors.json())
                );
        }
    }

    private parseErrors(errors: any)
    {
        this.formErrors.Model = errors[""] || [];
        this.formErrors.Username = errors["Username"] || [];
        this.formErrors.Email = errors["Email"] || [];
        this.formErrors.Password = errors["Password"] || [];
        this.formErrors.ConfirmPassword = errors["ConfirmPassword"] || [];
    }
}

class RegisterErrors
{
    Model: string[] = [];
    Username: string[] = [];
    Email: string[] = [];
    Password: string[] = [];
    ConfirmPassword: string[] = [];
}