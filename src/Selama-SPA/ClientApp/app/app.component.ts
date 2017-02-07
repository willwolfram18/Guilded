import { Component, ViewChild, AfterViewChecked } from "@angular/core";
import { MdProgressBar } from "@angular/material";
import { Router, NavigationStart, NavigationCancel, NavigationEnd, NavigationError } from "@angular/router";

import { ProgressBarService } from "./core/services/progress-bar";

@Component({
    selector: "app",
    template: require("./core/templates/app.component.html"),
    styles: [require("./core/templates/app.component.css")]
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
