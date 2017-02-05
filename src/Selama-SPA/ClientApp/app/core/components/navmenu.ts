import { Component } from "@angular/core";

import { AuthService } from "../services/auth";

@Component({
    selector: "nav-menu",
    template: require("../templates/navmenu.html"),
    styles: [require("../templates/navmenu.css")]
})
export class NavMenuComponent
{
    constructor(private authService: AuthService)
    {
    }

    public isLoggedIn(): boolean
    {
        return this.authService.isLoggedIn();
    }
}
