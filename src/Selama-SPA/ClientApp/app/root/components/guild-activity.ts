import { Component, AfterContentChecked, AfterContentInit, OnInit } from "@angular/core";
import { Http } from "@angular/http";

@Component({
    selector: "guild-activity",
    template: require("../templates/guild-activity.html"),
})
export class GuildActivityComponent implements OnInit
{
    public guildActivity: GuildActivity[];
    private oldListSize: number = 0;
    private page: number = 1;
    private $WowheadPower: any;
    private isLoading: boolean = false;

    constructor(private http: Http)
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
            this.http.get(`/api/guild-activity?page=${this.page}`).subscribe(result =>
            {
                if (!this.guildActivity)
                {
                    this.guildActivity = new Array<GuildActivity>();
                }
                this.guildActivity = this.guildActivity.concat(result.json() as GuildActivity[]);
                this.page++;
                this.isLoading = false;
            });
        }
    }
    
    private hasActivityRefreshed(): boolean
    {
        return this.guildActivity && this.guildActivity.length != this.oldListSize;
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
}

interface GuildActivity
{
    timestamp: Date;
    content: string;
}
