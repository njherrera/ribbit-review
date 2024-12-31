# Feature Overview

![image](https://github.com/user-attachments/assets/c6403608-8d71-4061-b5f2-f671dd71da98)

**Searches locally recorded or online replays for instances of edgeguard situations, with options for:** 

User's/the opponent’s edgeguards

Edgeguards that killed/didn’t kill

Selecting move used to send the opponent offstage (last move used before the opponent crossed over the ledge) 

**Filtering out replays, based on:**

User's character

Opponent’s character 

Stage

**Future plans:**

Searching for instances of CC/shield drop reverals + reversals out of juggles/player's conversions

Searching for dropped combos

Searching for getting hit in neutral while cornered/having center stage

# Design Info: 

Ribbit Review is split into 2 main parts and 2 languages in terms of the application itself, and will likely stay that way for some time (names themselves are also a work in progress).

The overall goal of the project is to build a multiplatform (Windows + MacOS) desktop app that parses stats generated from Smash Bros. Melee replays, queries the stats to find instances of user-specified situations, then replays them in Playback Dolphin.

In the long term, I also have plans to extend the same query logic to replay files of other players to find instances of the same situation where things go well, particularly replay files from the top level of competition (i.e. top 64 of a major tournament). For example, finding instances of a particular type of situation that end with the opponent being KO'd instead of surviving.

Initially the project used named pipes in order to exchange data between the C# parser/GUI and the slippi-js library, but it now uses the Jering.Javascript.NodeJS library for C#/JS interop functionality. The GUI/JavaScript folder contains the JS code used to perform basic replay filtering + grab stats from replays matching the user's query, which is then called in the GUI via JeringTech's interop library. In deployment, MSBuild also copies that JavaScript folder into the output. One area of improvement in this department is implementing webpack, and bundling those JS assets (along with the required node_modules folder and possibly a Node.js executable) together.  

## GUI:

References CSharpParser, and the project folder contains the JavaScript code used to get the conversion info from replays that we query to find situations. 

Besides being (of course) a GUI following the MVVM design pattern, it's responsible for calling the interop functionality (to obtain the required stats for parsing), then calling CSharpParser functionality to parse those stats and end up with a JSON file/Playback Queue object containing each situation matching the query.

In MVVM terms, the GUI is somewhat unique because it's concerned with View and View Model functionality, but not necessarily Model functionality. Instead, the views/view models of the GUI represent data structures and algorithmic parameters from the CSharpParser project (which is a class library).

## CSharpParser:

Class library that does not reference anything, and is responsible for parsing JSON files - converting them to C# objects and then analyzing those objects in order to do things like find edgeguard opportunities.

CSharpParser has representations of various data structures from slippi-js needed to make our queries, and also represents the stats/conversions from each game in a way that plays nice with C#. Additionally, it contains the logic for finding instances of different situations (represented as extensions of the abstract Filter class).

Essentially, CSharpParser is responsible for parsing on two fronts - parsing raw JSON-formatted data into workable C# data structures, and then parsing through those data structures in order to make queries for occurrences of specific situations.

Data structures from CSharpParser are also *used* by the GUI functionality to act as the model for its MVVM design, but the parser isn't inherently concerned with acting as a model for a GUI - it's entirely possible to use it by itself. 

## Program Flow

For applying a Filter to a .slp file/files (the core functionality), the process broadly looks like:

1. (GUI) Select replay file(s) in file explorer to apply the chosen filter to

2. (GUI.MainViewModel) GUI uses C#/JS interop functionality to call getAllConversions method from JavaScript/interop.js (called with `List<GameConversions>? requestedConversions = await GetAllConversions(args)`)

3. (GUI/JavaScript/interop.js) JS creates a SlippiGame from each selected file, then gets the conversions from it along with the game's settings info

4. (GUI.MainViewModel) `List<GameConversions>?` returned by interop call is then passed to the active Filter View Model's `applyFilter` method 

5. (GUI.FilterViewModel) Currently selected filter is applied to the conversions from above step, returning a PlaybackQueue object with timestamps for all matching conversions that's formatted to be readable by Slippi Playback Dolphin

6. (GUI.MainViewModel) File explorer window pops up to save the resulting PlaybackQueue object as a JSON file, which can then be played in Slippi Playback Dolphin
