import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { UniversalModule } from "angular2-universal";
import { SlimLoadingBarModule } from "ng2-slim-loading-bar";

import { ProgressBarService } from "./core/services/progress-bar";
import { RoutingModule } from "./core/modules/routing";

import { HomeAppModule } from "./home/modules/app";

import { AppComponent } from "./app.component"
import { NavMenuComponent } from "./core/components/navmenu";

import { HomeComponent as ForumsHomeComponent } from "./forums/components/home";

@NgModule({
    bootstrap: [
        AppComponent
    ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        ForumsHomeComponent,
    ],
    providers: [
        ProgressBarService,
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        SlimLoadingBarModule.forRoot(),
        RoutingModule,
        HomeAppModule,
    ]
})
export class AppModule {
}
