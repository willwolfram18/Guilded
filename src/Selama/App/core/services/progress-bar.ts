import { Injectable } from "@angular/core"
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
}