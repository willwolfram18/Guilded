"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var base_1 = require("../../../core/modules/base");
var routing_factory_1 = require("../../../core/services/routing-factory");
var footer_1 = require("../../../core/components/footer");
var menu_1 = require("../../../core/components/menu");
var app_1 = require("../components/app");
var home_1 = require("../components/home");
var routing_1 = require("./routing");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [
            base_1.BaseModule,
            routing_factory_1.RoutingFactoryService.create(routing_1.ROUTES),
        ],
        declarations: [
            app_1.AppComponent,
            home_1.HomeComponent,
            footer_1.FooterComponent,
            menu_1.MenuComponent,
        ],
        bootstrap: [
            app_1.AppComponent
        ],
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.js.map