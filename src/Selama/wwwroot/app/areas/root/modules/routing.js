"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var app_1 = require("../components/app");
var ROUTES = [
    {
        path: "",
        component: app_1.AppComponent,
    },
    {
        path: "home",
        component: app_1.AppComponent,
    },
];
var RoutingModule = RoutingModule_1 = (function () {
    function RoutingModule() {
    }
    return RoutingModule;
}());
RoutingModule = RoutingModule_1 = __decorate([
    core_1.NgModule({
        imports: [
            router_1.RouterModule.forRoot(ROUTES),
        ],
        exports: [
            RoutingModule_1,
        ]
    })
], RoutingModule);
exports.RoutingModule = RoutingModule;
var RoutingModule_1;
//# sourceMappingURL=routing.js.map