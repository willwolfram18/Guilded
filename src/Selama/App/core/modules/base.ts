import { NgModule } from "@angular/core"
import { MaterialModule } from "@angular/material"
import { BrowserModule } from "@angular/platform-browser"
import { SlimLoadingBarModule } from "ng2-slim-loading-bar"

import { ProgressBarService } from "../services/progress-bar"

@NgModule({
    imports: [
        BrowserModule,
        MaterialModule.forRoot(),
        SlimLoadingBarModule.forRoot(),
    ],
    providers: [
        ProgressBarService,
    ],
    exports: [
        BrowserModule,
        MaterialModule,
        SlimLoadingBarModule,
    ]
})
export class BaseModule
{
}