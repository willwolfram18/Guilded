import { Component } from "@angular/core";
import { Http } from "@angular/http";

@Component({
    selector: "guild-activity",
    template: require("../templates/guild-activity.html")
})
export class GuildActivityComponent
{
    public guildActivity: GuildActivity[];
    private page: number = 1;

    constructor(http: Http)
    {
        http.get(`/api/guild-activity?page=${this.page}`).subscribe(result =>
        {
            if (!this.guildActivity)
            {
                this.guildActivity = new Array<GuildActivity>();
            }
            this.guildActivity = this.guildActivity.concat(result.json() as GuildActivity[]);
        });
    }
}

interface GuildActivity
{
    timestamp: Date;
    content: string;
}
