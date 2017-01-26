"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var progress_bar_1 = require("../../../core/services/progress-bar");
var AppComponent = (function () {
    function AppComponent(progressBar, router) {
        var _this = this;
        this.progressBar = progressBar;
        this.router = router;
        this.progressBarSubscription = this.router.events.subscribe(function (e) {
            return _this.progressBar.handleRoutingEvent(e);
        }, function (e) { return _this.progressBar.complete(); });
    }
    AppComponent.prototype.ngOnDestroy = function () {
        this.progressBarSubscription.destroy();
    };
    return AppComponent;
}());
AppComponent = __decorate([
    core_1.Component({
        selector: "root-app",
        template: "<router-outlet name=\"menu\"></router-outlet>\n    <h1>Index page</h1>Tada!!\n    <br />\n    <router-outlet></router-outlet>\n    <router-outlet name=\"footer\"></router-outlet>"
    }),
    __metadata("design:paramtypes", [progress_bar_1.ProgressBarService,
        router_1.Router])
], AppComponent);
exports.AppComponent = AppComponent;
