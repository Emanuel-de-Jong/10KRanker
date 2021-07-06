# 10KRanker
A [discord bot](https://discord.com/developers/docs/intro#bots-and-apps) that keeps track of 10 key [mania](https://osu.ppy.sh/wiki/en/Game_mode#-osu!mania) maps that their mappers want to get [ranked](https://osu.ppy.sh/wiki/en/Beatmap/Category#ranked).
I will be used in the [10+ Key Rhythm Games](https://discord.gg/PwzcUzk) discord server.


## Commands
Discord users use commands to interact with the bot. All commands for this bot need to be prefixed with the symbol: '!'.<br/>
The commands are as follows:<br/>
`<>` = Required<br/>
`()` = Optional<br/>
`|` = Or<br/>
`""` = Send a name/title with spaces as 1 value<br/>

**Maps**<br/>
`!add`   `<map link|beatmapsetid>`   `(status)`<br/>
Add a map and describe in what stage of the mapping/ranking/modding proces it is.<br/>
`!rm`   `<map link|beatmapsetid|map title>`<br/>
Remove a map.<br/>
`!changestatus`   `<map link|beatmapsetid|map title>`   `<status>`<br/>
Change the status of a map. The status describes how the mapping/ranking/modding of a map is going.<br/>

**Beatmap Nominators**<br/>
`!addbn`   `<map link|beatmapsetid|map title>`   `<bn link|userid|bn name>`<br/>
Link a BN to a map. A map can have multiple BNs.<br/>
`!rmbn`   `<map link|beatmapsetid|map title>`   `<bn link|userid|bn name>`<br/>
Remove a BN from a map.<br/>

**Show maps**<br/>
`!show`   `<map link|beatmapsetid|map title>`<br/>
Show the detials of a map.<br/>
`!list`   `(user link|userid|user name)`<br/>
List all maps, the maps of a mapper or the maps linked to a BN.<br/>

**Other**<br/>
`!info`<br/>
Show this message.<br/>


## Code structure
There are 3 Visual Studio projects



- 10KRanked (C# .NET 5 Console Application)
- OsuAPI (C# .NET 5 Library)
- Database (C# .NET 5 Library)
