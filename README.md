# 10KRanker
A [discord bot](https://discord.com/developers/docs/intro#bots-and-apps) that keeps track of 10 key [mania](https://osu.ppy.sh/wiki/en/Game_mode#-osu!mania) maps that their mappers want to get [ranked](https://osu.ppy.sh/wiki/en/Beatmap/Category#ranked).
I will be used in the [10+ Key Rhythm Games](https://discord.gg/PwzcUzk) discord server.
It's good enough for what it will be used for. But for bigger projects, it would need some optimalizations. Especially in asynchronous programming.


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

### 10KRanked
**Type:** C# .NET 5 Console Application<br/>
**Dependencies:** [Discord.Net 2.4.0](https://www.nuget.org/packages/Discord.Net/2.4.0), [Microsoft.Extensions.DependencyInjection 5.0.1](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/5.0.1)

The discord bot itself. It uses Discord.Net so we don't have to worry about the connection to discord, reading and filtering command calls and parsing command parameters.
It uses the next 2 projects to retrieve and save all the data it needs.<br/>
There are 3 classes that have most of the logic:
**Modules/MainModule:** Handles the commands.<br/>
**Validator:** Most command parameters can be in multiple formats. This class gets the format type and converts the input accordingly.<br/>
**OsuToDB:** Combines the user input and info from OsuAPI to create and update Database entries.<br/>

### OsuAPI
**Type:** C# .NET 5 Library<br/>
**Dependencies:** [OsuSharp 5.4.4](https://www.nuget.org/packages/OsuSharp/5.4.4)

Has a static wrapper for easy communication with osu. 10KRanked uses it to get information about maps and users.
It throws exceptions when a map/user doesn't exist and if a map is incompatible with our standards.

### Database
**Type:** C# .NET 5 Library<br/>
**Dependencies:** [Microsoft.EntityFrameworkCore.Sqlite/Tools/Design 5.0.7](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.7)

Has a static wrapper for easy communication with a SQLite db file located in the OS's ApplicationData.
It uses auto generated migrations to speed up database design changes.
10KRanked uses it to save and retrieve the maps/users.

- 10KRanked (C# .NET 5 Console Application)
- OsuAPI (C# .NET 5 Library)
- Database (C# .NET 5 Library)
