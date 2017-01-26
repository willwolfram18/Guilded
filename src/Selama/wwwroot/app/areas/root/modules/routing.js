"use strict";
var home_1 = require("../components/home");
var about_1 = require("../components/about");
exports.ROUTES = [
    {
        path: "",
        component: home_1.HomeComponent,
    },
    {
        path: "home",
        component: home_1.HomeComponent,
    },
    {
        path: "home/about",
        component: about_1.AboutComponent,
    }
];
