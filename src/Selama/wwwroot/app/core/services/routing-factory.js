"use strict";
var router_1 = require("@angular/router");
var footer_1 = require("../components/footer");
var menu_1 = require("../components/menu");
var RoutingFactoryService = (function () {
    function RoutingFactoryService() {
    }
    RoutingFactoryService.create = function (appRoutes) {
        appRoutes.push(footer_1.FooterComponent.FooterRoute);
        appRoutes.push(menu_1.MenuComponent.MenuRoute);
        return router_1.RouterModule.forRoot(appRoutes);
    };
    return RoutingFactoryService;
}());
exports.RoutingFactoryService = RoutingFactoryService;
