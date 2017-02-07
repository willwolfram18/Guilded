import { Injectable } from "@angular/core";
import { Router, CanActivate } from "@angular/router";

import { AuthService } from "../core/auth.service";

@Injectable()
export class SignInGuard implements CanActivate
{
    constructor(private router: Router, private authService: AuthService)
    {
    }

    public canActivate()
    {
        let isLoggedIn = this.authService.isLoggedIn;
        if (isLoggedIn)
        {
            this.router.navigate(["/home"]);
        }
        return !isLoggedIn;
    }
}