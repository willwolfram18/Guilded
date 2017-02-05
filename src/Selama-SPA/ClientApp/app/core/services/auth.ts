import { Injectable } from "@angular/core";
import { Http, RequestOptionsArgs } from "@angular/http";
import { AuthHttp, JwtHelper, tokenNotExpired } from "angular2-jwt";

const TOKEN_KEY = "id_token";

@Injectable()
export class AuthService
{
    private logInUrl: string = "/api/account/sign-in";
    private logOutUrl: string = "/api/account/sign-out";
    private jwtHelper: JwtHelper = new JwtHelper();

    constructor(private http: Http, private authHttp: AuthHttp)
    {
    }

    public logIn(emailAddress: string, password: string, rememberMe: boolean)
    {
        let logInObservable = this.http.post(this.logInUrl, { Email: emailAddress, Password: password, RememberMe: rememberMe })
        logInObservable.subscribe(result =>
        {
            var accessToken: AccessToken = result.json() as AccessToken;
            localStorage.setItem(TOKEN_KEY, accessToken.access_token);
        });
        return logInObservable;
    }

    public logOut()
    {
        if (!this.isLoggedIn())
        {
            return null;
        }
        let logOutObservable = this.authHttp.post(this.logOutUrl, {});
        logOutObservable.subscribe(result =>
        {
            localStorage.removeItem(TOKEN_KEY);    
        });
        return logOutObservable;
    }

    public isLoggedIn(): boolean
    {
        return tokenNotExpired();
    }

    public get(url: string, options?: RequestOptionsArgs)
    {
        if (this.isLoggedIn())
        {
            return this.authHttp.get(url, options);
        }
        else
        {
            return this.http.get(url, options);
        }
    }
    public post(url: string, body: any, options?: RequestOptionsArgs)
    {
        if (this.isLoggedIn())
        {
            return this.authHttp.post(url, body, options);
        }
        else
        {
            return this.http.post(url, body, options);
        }
    }
}

interface AccessToken
{
    access_token: string;
    expires_in: number;
}