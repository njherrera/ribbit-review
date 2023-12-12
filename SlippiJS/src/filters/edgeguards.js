'use strict'
Object.defineProperty(exports, '__esModule', { value: true })
var slippi_js_1 = require('@slippi/slippi-js')
var fs = require('fs-extra')
var path = require('path')
// is this helper method even needed?
// function checkConversionForEdgeugard(conversion: ConversionType): boolean {
//     let isEdgeguard: boolean = false;
//     if (checkEdgeguardPosition(conversion) == true){
//         isEdgeguard = true;
//     }
//     return isEdgeguard;
// }
// takes a stageID as input param, returns 2-item array of x positions for each ledge
// returns null if stageID doesn't match a legal stage
function getLedgeCoords(stageID) {
    switch (true) {
        // FoD
        case stageID == 2: {
            return [-63.35, 63.35]
            break
        }
        // Stadium
        case stageID == 3: {
            return [-87.75, 87.75]
            break
        }
        // Yoshi's
        case stageID == 8: {
            return [-56, 56]
            break
        }
        // Dream Land
        case stageID == 28: {
            return [-77.27, 77.27]
            break
        }
        // Battlefield
        case stageID == 31: {
            return [-68.4, 68.4]
            break
        }
        // FD
        case stageID == 32: {
            return [-85.57, 85.57]
            break
        }
        default: {
            return null
        }
    }
}
// checks to see if there's an edgeguard position somewhere in the conversion
// helper method for use with checkConversionForEdgeguard
// refactor a bit, move ledge coord variables to getEdgeguards?
function checkEdgeguardPosition(game, conversion) {
    // edgeguardedIndex is the index of the player who's getting edgeguarded/hit, edgeguardingIndex is the same but for the player doing the edgeguard
    var edgeguardedIndex = conversion.playerIndex
    var edgeguardingIndex = conversion.lastHitBy
    // get frames from game matching conversion
    var frames = game.getFrames()
    var firstConversionFrame = conversion.startFrame
    var lastConversionFrame = conversion.endFrame
    var settings = game.getSettings()
    // initialize ledge coords for stage the game was played on
    var ledgeCoords = getLedgeCoords(settings.stageId)
    var leftLedge = ledgeCoords[0]
    var rightLedge = ledgeCoords[1]
    var isEdgeGuardPosition = false
    // for loop that goes through frames of conversion and checks to see if there was an edgeguard position on any frame
    // edgeguard position currently defined as conversion victim being offstage, and converting player being closer to stage by comparison
    for (
        var currentFrame = firstConversionFrame;
        currentFrame <= lastConversionFrame;
        currentFrame++
    ) {
        var edgeguardVictimXPos =
            frames[currentFrame].players[edgeguardedIndex].post.positionX
        var edgeguarderXPos =
            frames[currentFrame].players[edgeguardingIndex].post.positionX
        if (
            (edgeguardVictimXPos < leftLedge &&
                edgeguarderXPos > edgeguardVictimXPos) ||
            (edgeguardVictimXPos > rightLedge &&
                edgeguarderXPos < edgeguardVictimXPos)
        ) {
            isEdgeGuardPosition = true
        }
    }
    return isEdgeGuardPosition
}
// function to take all conversions  in a game, then filter out the non-edgeguards
// returns a JSON file!
function getEdgeguards(replayPath, outputJSON) {
    var game = new slippi_js_1.SlippiGame(replayPath)
    // initialize game stats
    var stats = game.getStats()
    // initialize game conversions
    var conversions =
        stats === null || stats === void 0 ? void 0 : stats.conversions
    // go through each conversion and evaluate it against edgeguard criteria
    // if it meets criteria, add it to JSON file
    conversions.forEach(function (conversion) {
        if (checkEdgeguardPosition(game, conversion) == true) {
            var queueItem = {
                path: game.getFilePath(),
                startFrame: conversion.startFrame,
                endFrame: conversion.endFrame,
            }
            outpu
            tJSON.queue.push(queueItem)
        }
    })
}
// function to apply getEdgeguards to a folder of .slp files
// to-do: move to filter interface, add safeguards that skip over non-.slp files, write tests that check only for reading .slp files
function readSlpFolder(dirName, outputJSON) {
    fs.readdir(dirName, function (error, fileNames) {
        if (error) {
            console.log(dirName)
            throw error
        } else {
            fileNames.forEach(function (fileName) {
                var filePath = path.resolve(process.cwd(), '../.slp files/', fileName)
                //let filePath = path.parse(fileName).name + ".slp";
                getEdgeguards(filePath, outputJSON)
            })
        }
    })
    return outputJSON
}
var testJSON = { mode: 'queue', queue: [] }
var dirPath = path.resolve(process.cwd() + '/.slp files/')
var writePath = path.resolve(process.cwd() + '/JSON files/')
fs.writeFile(
    writePath + 'test2.json',
    JSON.stringify(readSlpFolder(dirPath, testJSON)),
    function (error) {
        if (error) {
            console.error(error)
            throw error
        }
        console.log('json written correctly')
    }
)
// Get frames – animation state, inputs, etc
// This is used to compute your own stats or get more frame-specific info (advanced)
// const frames_ = game.getFrames();
// console.log(frames_[0].players); // Print frame when timer starts counting down
//console.log(stats.conversions);
