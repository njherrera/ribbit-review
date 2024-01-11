# Design Info: 

ribbit-review is split into 4 main parts and 2 languages in terms of the application itself, and will likely stay that way for some time (names themselves are also a work in progress):

## CSharpGUI:

Has references to CSharpParser and NamedPipeAPI but *not* JS code, and is currently the default testing ground for C# logic but in future will handle the UI.

## CSharpParser:

Does not reference anything, and is responsible for parsing JSON files - converting them to C# objects and then analyzing those objects in order to do things like find edgeguard opportunities.

## NamedPipeAPI

Does not reference anything, and is responsible for operating the two named pipes that tie the C# code to the JS code. One pipe (Pipe A/Request Pipe) sends a request for a JSON file(s) describing a given replay/set of replays to JS, and the other receives said JSON file(s) from JS. 
Specifics of how the pipes operate and what the JSON files look like are subject to change, but the basic principles will (likely) stay the same throughout the project. 

This is also what allows the other C# parts of the project to not care about JS at all, and itself doesn't have any JS code.

## SlippiJS

Responsible for the other half of pipe operations, also creating JSON-like objects from .slp files to send through Pipe B.

Does also contain a "filters" folder with logic for finding edgeguard opportunities, but that's more for easy reference and filter logic will be handled by C#.
