import { Component, ViewChild, AfterViewChecked} from "@angular/core";

import { GuildActivityComponent } from "./guild-activity.component";

@Component({
    selector: "home",
    template: require("./templates/home.html")
})
export class HomeComponent implements AfterViewChecked
{
    @ViewChild(GuildActivityComponent)
    guildActivityComponent: GuildActivityComponent;

    public ngAfterViewChecked() : void 
    {
        this.guildActivityComponent.refreshLinks();
    }
}
