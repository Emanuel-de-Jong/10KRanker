# 10KRanker
A [discord bot](https://discord.com/developers/docs/intro#bots-and-apps) that keeps track of 10 key [mania](https://osu.ppy.sh/wiki/en/Game_mode#-osu!mania) maps that their mappers want to get [ranked](https://osu.ppy.sh/wiki/en/Beatmap/Category#ranked).

I will be used it in the [10+ Key Rhythm Games](https://discord.gg/PwzcUzk) discord server.

It's good enough for what it will be used for. But for bigger projects, it would need some optimalizations. Especially in asynchronous programming.

**Active Development:** 2021-06-30 - 2021-07-11<br>
**Last Change:** 2021-10-30<br>

| | |
| :---: | :---: |
| ![](/Screenshots/1-Help_Command.png) | ![](/Screenshots/2-List_Command.png) |

## Commands
Discord users use commands to interact with the bot. All commands for this bot need to be prefixed with: `!`<br/>
The commands are as follows:<br/>
`<>` = Required<br/>
`()` = Optional<br/>
`|` = Or<br/>
`""` = Send a name/title with spaces as 1 value<br/>

***Maps***<br/>
`!add`   `<map link|beatmapsetid>`   `(status)`<br/>
Add a map and describe in what stage of the mapping/ranking/modding proces it is.<br/>
`!rm`   `<map link|beatmapsetid|map title>`<br/>
Remove a map.<br/>
`!changestatus`   `<map link|beatmapsetid|map title>`   `<status>`<br/>
Change the status of a map. The status describes how the mapping/ranking/modding of a map is going.<br/>

***Beatmap Nominators***<br/>
`!addbn`   `<map link|beatmapsetid|map title>`   `<bn link|userid|bn name>`<br/>
Link a BN to a map. A map can have multiple BNs.<br/>
`!rmbn`   `<map link|beatmapsetid|map title>`   `<bn link|userid|bn name>`<br/>
Remove a BN from a map.<br/>

***Show maps***<br/>
`!show`   `<map link|beatmapsetid|map title>`<br/>
Show the details of a map.<br/>
`!list`   `(user link|userid|user name)`<br/>
List all maps, the maps of a mapper or the maps linked to a BN.<br/>

***Other***<br/>
`!info`<br/>
Show this message.<br/>

## Issues
***I get the error: `The type initializer for 'Database.DB' threw an exception.` for every command.***<br/>
The Database project (Entity Framework Core) can't connect to the db file. Probably because the data source configuration has changed.<br/>
A new migration has to be generated:
1. Delete the Migrations folder.
2. Set the startup project to the Database project.
3. Open the Package Manager Console (PMC) in VS.
4. Give in the following commands:
    - `Add-Migration YourMigrationName`
    - `Update-Database`
5. Set the startup project back to the 10KRanker project.

## Code structure
There are 5 Visual Studio projects

### 10KRanked
***Type:*** C# .NET 5 Console Application<br/>
***Dependencies:*** [Discord.Net 2.4.0](https://www.nuget.org/packages/Discord.Net/2.4.0), [Microsoft.Extensions.DependencyInjection 5.0.1](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/5.0.1)

The discord bot itself. It uses Discord.Net so we don't have to worry about the connection to discord, reading and filtering command calls and parsing command parameters. It uses the next 2 projects to retrieve and save all the data it needs.<br/>
There are 3 classes that have most of the logic:<br/>
***Modules/MainModule:*** Handles the commands.<br/>
***Validator:*** Most command parameters can be in multiple formats. This class gets the format type and converts the input accordingly.<br/>
***OsuToDB:*** Combines the user input and info from OsuAPI to create and update Database entries.<br/>

E.g adding a map would look like this (pseudo naming):<br/>
Validator.GetMapType(Input) -> Validator.MapTypeXToId(MapType)<br/>
-> (OsuToDB.CreateMap(MapId) -> Osu.GetMap(MapId)) -> DB.Add(Map)

### OsuAPI
***Type:*** C# .NET 5 Library<br/>
***Dependencies:*** [OsuSharp 5.4.4](https://www.nuget.org/packages/OsuSharp/5.4.4)

Has a static wrapper for easy communication with osu. 10KRanked uses it to get information about maps and users. It throws exceptions when a map/user doesn't exist and if a map is incompatible with our standards.

### Database
***Type:*** C# .NET 5 Library<br/>
***Dependencies:*** [Microsoft.EntityFrameworkCore.Sqlite/Tools/Design 5.0.7](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.7)

Has a static wrapper for easy communication with a SQLite db file located in the OS's ApplicationData. It uses auto generated migrations to speed up database design changes.
10KRanked uses it to save and retrieve the maps/users.

### Logger
***Type:*** C# .NET 5 Library<br/>

Its Log class can be instanced with a filename. The Write function appends a string to the `.log` file. It is used to log the user commands, osu api calls and database changes.

### Test
***Type:*** C# .NET 5 Console Application<br/>

Used to test individual features without the overhead and complexity of the discord bot.

## Running your own version
I'm asuming you are using Visual Studio 2019.

1. Fill in the secret templates and rename their file and class name to `Secret`.
    - [10KRanked template](https://github.com/Emanuel-de-Jong/10KRanker/blob/7950235b3674a0618c2475c4ce9c88a4d7d1e8dc/src/10KRanker/SecretsTemplate.cs)
    - [OsuAPI template](https://github.com/Emanuel-de-Jong/10KRanker/blob/7950235b3674a0618c2475c4ce9c88a4d7d1e8dc/src/OsuAPI/SecretsTemplate.cs)
2. That is it for now.

## Testing
The 10KRanked project has a UnitTest class that acts as a discord user and calls commands. It goes through most of the possible and invalid parameters of the commands.<br/>
Before testing, make a backup of your db file and put an empty db file in its place.<br/>
To start the test, set the UnitTest Testing bool to true, and run the bot.
