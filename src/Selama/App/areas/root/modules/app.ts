import { NgModule } from "@angular/core"

import { BaseModule } from "../../../core/modules/base"

import { AppComponent } from "../components/app"

@NgModule({
    imports: [BaseModule],
    declarations: [
        AppComponent,
    ],
    bootstrap: [
        AppComponent
    ],
})
export class AppModule
{
}