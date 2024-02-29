# Design Info: 

Ribbit Review is split into 4 main parts and 2 languages in terms of the application itself, and will likely stay that way for some time (names themselves are also a work in progress). 

## GUI:

Has references to CSharpParser and NamedPipeAPI but *not* JS code, and is what handles the GUI (of course). The GUI is what calls CSharpParser and NamedPipeAPI methods, and is the "main" C# project in terms of getting everything else rolling.

## CSharpParser:

Does not reference anything, and is responsible for parsing JSON files - converting them to C# objects and then analyzing those objects in order to do things like find edgeguard opportunities.

## NamedPipeAPI

Does not reference anything, and is responsible for operating the two named pipes that tie the C# code to the JS code. One pipe (Pipe A/Request Pipe) sends a request for a JSON file(s) describing a given replay/set of replays to JS, and the other receives said JSON file(s) from JS. 
Specifics of how the pipes operate and what the JSON files look like are subject to change, but the basic principles will (likely) stay the same throughout the project. 

This is also what allows the other C# parts of the project to not care about JS at all, and itself doesn't have any JS code.

## SlippiJS

Responsible for the other half of pipe operations, also creating JSON-like objects from .slp files to send through Pipe B.

Does also contain a "Filters" folder with logic for finding edgeguard opportunities, but that's more for easy reference and filter logic will be handled by C#.

## Program Flow

For applying a Filter to a .slp file or files (the core functionality), the process broadly looks like:

1. (GUI) Select replay file(s) in file explorer to apply the chosen filter to

2. (NamedPipeAPI.PipeManager) C# sends that file's URL through named pipe 1 to JS

3. (SlippiJS.PipeManager) JS receives the file URL through named pipe 1

4. (SlippiJS.RequestManager) JS creates a SlippiGame from the file URL read into a buffer, then gets the conversions from it

5. (SlippiJS.PipeManager) JS converts the conversions to JSON format and sends them through named pipe 2 to C#

6. (NamedPipeAPI.PipeManager) C# receives the JSON-formatted string of conversions through named pipe 2

7. (CSharpParser) C# translates the JSON conversion list into C# types 

8. (CSharpParser) C# applies whatever filter is currently selected in the GUI to the conversion list

9. (GUI) Another file explorer window pops up to name and save the resulting JSON file
