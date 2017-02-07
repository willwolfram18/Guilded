import { Injectable } from "@angular/core";
import { MdProgressBar } from "@angular/material";
import { NavigationStart, NavigationCancel, NavigationEnd, NavigationError, RoutesRecognized } from "@angular/router";
import { SlimLoadingBarService } from "ng2-slim-loading-bar";

@Injectable()
export class ProgressBarService
{
    private static progressBar: MdProgressBar;

    public setProgressBar(progressBar: MdProgressBar)
    {
        ProgressBarService.progressBar = progressBar;
    }

    public start(): void
    {
        ProgressBarService.progressBar.mode = "indeterminate";
    }

    public complete(): void
    {
        ProgressBarService.progressBar.mode = "determinate";
        ProgressBarService.progressBar.value = 0;
    }

    public reset(): void
    {
        this.start();
    }

    public stop(): void
    {
        ProgressBarService.progressBar.mode = "determinate";
        ProgressBarService.progressBar.value = (Math.random() * 10000) % 100;
    }
}