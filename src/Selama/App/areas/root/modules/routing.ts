import { Route } from "@angular/router"

import { HomeComponent } from "../components/home"

export const ROUTES: Route[] = [
    {
        path: "",
        component: HomeComponent,
    },
    {
        path: "home",
        component: HomeComponent,
    },
];