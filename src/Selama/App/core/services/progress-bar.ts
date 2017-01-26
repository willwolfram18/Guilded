import { Injectable } from "@angular/core"
import { Router, NavigationCancel, NavigationEnd, NavigationError, NavigationStart, RoutesRecognized } from "@angular/router"
import { SlimLoadingBarService } from "ng2-slim-loading-bar"

@Injectable()
export class ProgressBarService
{
    constructor(private progressBar: SlimLoadingBarService)
    {
    }

    start(): void
    {
        this.progressBar.start();
    }

    complete(): void
    {
        this.progressBar.complete();
    }

    incrementProgress(progress: number): void
    {
        this.progressBar.progress += progress;
    }

    setProgress(progress: number): void
    {
        this.progressBar.progress = progress;
    }

    stop(): void
    {
        this.progressBar.stop();
    }

    handleRoutingEvent(event: NavigationStart | NavigationCancel | NavigationEnd | NavigationError | RoutesRecognized)  
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