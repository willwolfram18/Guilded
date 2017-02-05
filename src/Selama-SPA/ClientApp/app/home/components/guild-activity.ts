import { Component, AfterContentChecked, AfterContentInit, OnInit } from "@angular/core";

import { AuthService } from "../../core/services/auth";

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

    constructor(private authService: AuthService)
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
                this.authService.get(`/api/guild-activity?page=${this.page}`)
                    .finally(() => this.isLoading = false)
                    .subscribe(result =>
                    {
                        this.guildActivity = this.guildActivity.concat(result.json() as GuildActivity[]);
                        this.page++;
                    });
            }
            else
            {
                this.authService.get("/api/guild-activity/test")
                    .finally(() => this.isLoading = false)
                    .subscribe(result =>
                        console.log(result)
                    );
            }
        }
    }

    public isLoggedIn(): boolean
    {
        return this.authService.isLoggedIn();
    }

    public logIn(): void
    {
        this.authService.logIn("wolfington98@gmail.com", "@bcXyz123", false)
    }
    public logOut(): void
    {
        this.authService.logOut();
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
