import { Component, OnDestroy } from "@angular/core"
import { Route, Router, NavigationCancel, NavigationEnd, NavigationError, NavigationStart, RoutesRecognized } from "@angular/router"


import { ProgressBarService } from "../../../core/services/progress-bar"

@Component({
    selector: "root-app",
    template: `<router-outlet name="menu"></router-outlet>
    <h1>Index page</h1>Tada!!
    <br />
    <router-outlet></router-outlet>
    <router-outlet name="footer"></router-outlet>`
})
export class AppComponent implements OnDestroy
{
    private progressBarSubscription: any;

    constructor(
        private progressBar: ProgressBarService,
        private router: Router
    )
    {
        this.progressBarSubscription = this.router.events.subscribe(e =>
            this.progressBar.handleRoutingEvent(e),
            e => this.progressBar.complete()
        );
    }

    ngOnDestroy(): void
    {
        this.progressBarSubscription.destroy();
    }
}