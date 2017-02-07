import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { UniversalModule } from "angular2-universal";
import { SlimLoadingBarModule } from "ng2-slim-loading-bar";

import { AuthModule } from "./core/auth.module";
import { ProgressBarService } from "./core/progress-bar.service";

import { HomeAppModule } from "./home/app.module";
import { ForumsAppModule } from "./forums/app.module";
import { AccountModule } from "./account/app.module";

import { AppComponent } from "./app.component"
import { NavMenuComponent } from "./core/navmenu.component";

@NgModule({
    bootstrap: [
        AppComponent
    ],
    declarations: [
        AppComponent,
        NavMenuComponent,
    ],
    providers: [
        ProgressBarService,
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        SlimLoadingBarModule.forRoot(),
        RouterModule.forRoot([]),
        AuthModule,
        HomeAppModule,
        ForumsAppModule,
        AccountModule,
    ]
})
export class AppModule {
}
