import { Injectable } from "@angular/core";
import { Http, RequestOptionsArgs, Response } from "@angular/http";
import { AuthHttp, JwtHelper, tokenNotExpired } from "angular2-jwt";

const TOKEN_KEY = "id_token";

export class RegisterUserModel
{
    Username: string;
    Email: string;
    Password: string;
    ConfirmPassword: string;
}
export class SignInModel
{
    Email: string;
    Password: string;
    RememberMe: boolean = false;
}

@Injectable()
export class AuthService
{
    private logInUrl: string = "/api/account/sign-in";
    private logOutUrl: string = "/api/account/sign-out";
    private registerUrl: string = "/api/account/register";
    private jwtHelper: JwtHelper = new JwtHelper();

    constructor(private http: Http, private authHttp: AuthHttp)
    {
    }

    public logIn(user: SignInModel)
    {
        let logInObservable = this.http.post(this.logInUrl, user).share();
        return logInObservable.do(result => this.storeAccessToken(result));
    }
    private storeAccessToken(result: Response)
    {
        let accessToken: AccessToken = result.json() as AccessToken;
        localStorage.setItem(TOKEN_KEY, accessToken.access_token);
    }

    public register(user: RegisterUserModel)
    {
        let registerObservable = this.http.post(this.registerUrl, user).share();
        return registerObservable.do(result => this.storeAccessToken(result));
    }

    public logOut()
    {
        if (!this.isLoggedIn())
        {
            return null;
        }
        let logOutObservable = this.authHttp.post(this.logOutUrl, {}).share();
        return logOutObservable.do(result =>
        {
            localStorage.removeItem(TOKEN_KEY);    
        });
    }

    public isLoggedIn(): boolean
    {
        return tokenNotExpired();
    }

    public get(url: string, options?: RequestOptionsArgs)
    {
        if (this.isLoggedIn())
        {
            return this.authHttp.get(url, options).share();
        }
        else
        {
            return this.http.get(url, options).share();
        }
    }

    public post(url: string, body: any, options?: RequestOptionsArgs)
    {
        if (this.isLoggedIn())
        {
            return this.authHttp.post(url, body, options).share();
        }
        else
        {
            return this.http.post(url, body, options).share();
        }
    }
}

interface AccessToken
{
    access_token: string;
    expires_in: number;
}