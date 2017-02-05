import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AuthService } from "../services/auth";

@Component({
    selector: "nav-menu",
    template: require("../templates/navmenu.html"),
    styles: [require("../templates/navmenu.css")]
})
export class NavMenuComponent
{
    constructor(private authService: AuthService, private router: Router)
    {
    }

    public isLoggedIn(): boolean
    {
        return this.authService.isLoggedIn();
    }

    public logOut(): void
    {
        if (this.isLoggedIn())
        {
            this.authService.logOut()
                .subscribe(result => this.router.navigate(["/home"]));
        }
    }
}
