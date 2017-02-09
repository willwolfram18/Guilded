import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AuthService } from "./auth.service";

@Component({
    selector: "nav-menu",
    template: require("./templates/navmenu.html"),
    styles: [require("./templates/navmenu.scss")]
})
export class NavMenuComponent
{
    constructor(private authService: AuthService, private router: Router)
    {
    }

    public get isLoggedIn(): boolean
    {
        return this.authService.isLoggedIn;
    }

    public logOut(): void
    {
        if (this.isLoggedIn)
        {
            this.authService.logOut()
                .subscribe(result => this.router.navigate(["/home"]));
        }
    }
}
