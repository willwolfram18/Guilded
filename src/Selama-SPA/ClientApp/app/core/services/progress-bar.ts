import { Injectable } from "@angular/core";
import { NavigationStart, NavigationCancel, NavigationEnd, NavigationError, RoutesRecognized } from "@angular/router";
import { SlimLoadingBarService } from "ng2-slim-loading-bar";

@Injectable()
export class ProgressBarService
{
    constructor(private progressBar: SlimLoadingBarService)
    {
    }

    public start(): void
    {
        this.progressBar.start();
    }

    public complete(): void
    {
        this.progressBar.complete();
    }

    public reset(): void
    {
        this.progressBar.reset();
    }

    public stop(): void
    {
        this.progressBar.stop();
    }

    public handleRoutingEvent(event: NavigationStart | NavigationCancel | NavigationEnd | NavigationError | RoutesRecognized): void
    {
        if (event instanceof NavigationStart)
        {
            this.start();
        }
        else if (
            event instanceof NavigationCancel ||
            event instanceof NavigationEnd ||
            event instanceof NavigationError
        )
        {
            this.complete();
        }
    }
}