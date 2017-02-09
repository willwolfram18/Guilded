import { Component, ViewChild, AfterViewChecked } from "@angular/core";
import { MdProgressBar } from "@angular/material";
import { Router, NavigationStart, NavigationCancel, NavigationEnd, NavigationError } from "@angular/router";

import { ProgressBarService } from "./core/progress-bar.service";

@Component({
    selector: "app",
    template: require("./core/templates/app.component.html"),
    styles: [require("./core/templates/app.component.scss")]
})
export class AppComponent implements AfterViewChecked
{
    @ViewChild(MdProgressBar)
    progressBar: MdProgressBar;

    constructor(private progressBarService: ProgressBarService)
    {
    }

    ngAfterViewChecked()
    {
        this.progressBarService.setProgressBar(this.progressBar);
    }
}
