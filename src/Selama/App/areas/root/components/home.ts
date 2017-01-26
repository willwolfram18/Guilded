import { Component } from "@angular/core"

@Component({
    selector: "home",
    template: `<h3>This is the home component</h3><a routerLink='home/about'>About us</a>`,
})
export class HomeComponent
{
}