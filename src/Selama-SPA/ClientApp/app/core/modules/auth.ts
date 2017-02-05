import { NgModule } from "@angular/core";
import { Http, RequestOptions } from "@angular/http";
import { AuthHttp, AuthConfig } from "angular2-jwt";

import { AuthService } from "../services/auth";

const AUTH_CONFIG = new AuthConfig();

function authHttpServiceFactory(http: Http, options: RequestOptions): AuthHttp
{
    return new AuthHttp(AUTH_CONFIG, http, options);
}

@NgModule({
    providers: [
        {
            provide: AuthHttp,
            useFactory: authHttpServiceFactory,
            deps: [
                Http,
                RequestOptions,
            ]
        },
        AuthService,
    ],
})
export class AuthModule
{
}