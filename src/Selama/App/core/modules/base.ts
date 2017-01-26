import { NgModule } from "@angular/core"
import { MaterialModule } from "@angular/material"
import { BrowserModule } from "@angular/platform-browser"
import { RouterModule } from "@angular/router"
import { SlimLoadingBarModule } from "ng2-slim-loading-bar"

import { FooterComponent } from "../components/footer"
import { MenuComponent } from "../components/menu"
import { ProgressBarService } from "../services/progress-bar"

@NgModule({
    imports: [
        BrowserModule,
        MaterialModule.forRoot(),
        RouterModule,
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