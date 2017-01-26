import { Route } from "@angular/router"

import { AppComponent } from "../components/app"

export const ROUTES: Route[] = [
    {
        path: "",
        component: AppComponent,
    },
    {
        path: "home",
        component: AppComponent,
    },
];