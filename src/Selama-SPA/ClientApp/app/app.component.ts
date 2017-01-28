import { Component } from '@angular/core';
import { ProgressBarService } from "./core/services/progress-bar";

@Component({
    selector: 'app',
    template: require('./core/templates/app.component.html'),
    styles: [require('./core/templates/app.component.css')]
})
export class AppComponent
{
    constructor(private progressBar: ProgressBarService)
    {

    }
}
