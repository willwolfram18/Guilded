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
var ng2_slim_loading_bar_1 = require("ng2-slim-loading-bar");
var ProgressBarService = (function () {
    function ProgressBarService(progressBar) {
        this.progressBar = progressBar;
    }
    ProgressBarService.prototype.start = function () {
        this.progressBar.start();
    };
    ProgressBarService.prototype.complete = function () {
        this.progressBar.complete();
    };
    ProgressBarService.prototype.incrementProgress = function (progress) {
        this.progressBar.progress += progress;
    };
    ProgressBarService.prototype.setProgress = function (progress) {
        this.progressBar.progress = progress;
    };
    ProgressBarService.prototype.stop = function () {
        this.progressBar.stop();
    };
    ProgressBarService.prototype.handleRoutingEvent = function (event) {
        if (event instanceof router_1.NavigationStart) {
            this.start();
        }
        else if (event instanceof router_1.NavigationCancel ||
            event instanceof router_1.NavigationEnd ||
            event instanceof router_1.NavigationError) {
            this.complete();
        }
    };
    ProgressBarService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [ng2_slim_loading_bar_1.SlimLoadingBarService])
    ], ProgressBarService);
    return ProgressBarService;
}());
exports.ProgressBarService = ProgressBarService;
