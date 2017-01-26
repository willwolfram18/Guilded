import { Component } from "@angular/core"

@Component({
    selector: "root-app",
    template: `<router-outlet name="menu"></router-outlet>
    <h1>Index page</h1>Tada!!
    <br />
    <router-outlet></router-outlet>
    <router-outlet name="footer"></router-outlet>`
})
export class AppComponent
{
}