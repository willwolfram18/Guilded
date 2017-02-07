import { Injectable } from "@angular/core";
import { CanActivateChild, Router } from "@angular/router";

import { AuthService } from "../../../../core/services/auth";

@Injectable()
export class ManagerGuard implements CanActivateChild
{
    constructor(private router: Router, private authService: AuthService)
    {
    }

    public canActivateChild()
    {
        let isLoggedIn = this.authService.isLoggedIn;
        if (!isLoggedIn)
        {
            this.router.navigate(["/account/sign-in"]);
        }
        return isLoggedIn;
    }
}