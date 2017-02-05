import { Component, AfterContentChecked, AfterContentInit, OnInit } from "@angular/core";
import { Http } from "@angular/http";
import { AuthHttp, tokenNotExpired, JwtHelper } from "angular2-jwt";

@Component({
    selector: "guild-activity",
    template: require("../templates/guild-activity.html"),
    styles: [
        require("../templates/guild-activity.css"),
    ],
})
export class GuildActivityComponent implements OnInit
{
    public guildActivity: GuildActivity[] = Array<GuildActivity>();
    private oldListSize: number = 0;
    private page: number = 1;
    private $WowheadPower: any;
    private isLoading: boolean = false;

    constructor(private http: Http, private authHttp: AuthHttp)
    {
        this.$WowheadPower = window["$WowheadPower"];
    }

    ngOnInit()
    {
        this.loadItems();
    }

    public loadItems(): void
    {
        if (!this.isLoading)
        {
            this.isLoading = true;
            if (!this.isLoggedIn())
            {
                this.http.get(`/api/guild-activity?page=${this.page}`)
                    .finally(() => this.isLoading = false)
                    .subscribe(result =>
                    {
                        this.guildActivity = this.guildActivity.concat(result.json() as GuildActivity[]);
                        this.page++;
                    });
            }
            else
            {
                this.authHttp.get("/api/guild-activity/test")
                    .subscribe(result =>
                        console.log(result)
                    );
            }
        }
    }

    public isLoggedIn(): boolean
    {
        return tokenNotExpired();
    }

    public logIn(): void
    {
        this.http.post("/api/account/sign-in", { Email: "wolfington98@gmail.com", Password: "@bcXyz123", })
            .subscribe(result =>
            {
                let token = result.json();
                localStorage.setItem("id_token", token.access_token);
                let j = new JwtHelper();
                console.log(j.decodeToken(token.access_token), j.getTokenExpirationDate(token.access_token), j.isTokenExpired(token.access_token))
            });
    }

    public refreshLinks(): void
    {
        if (this.hasActivityRefreshed())
        {
            this.oldListSize = this.guildActivity.length;
            if (this.$WowheadPower && this.$WowheadPower.refreshLinks)
            {
                console.log("refreshing");
                this.$WowheadPower.refreshLinks();
            }
        }
    }
    private hasActivityRefreshed(): boolean
    {
        return this.guildActivity && this.guildActivity.length != this.oldListSize;
    }
}

interface GuildActivity
{
    timestamp: Date;
    content: string;
}
