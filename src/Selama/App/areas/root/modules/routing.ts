import { Route } from "@angular/router"

import { HomeComponent } from "../components/home"
import { AboutComponent } from "../components/about"

export const ROUTES: Route[] = [
    {
        path: "",
        component: HomeComponent,
    },
    {
        path: "home",
        component: HomeComponent,
    },
    {
        path: "home/about",
        component: AboutComponent,
    }
];