const { SlippiGame } = require("@slippi/slippi-js");
const { readFileSync } = require("fs");

/**
 * @param {string} location file location of replay that c# is requesting in JSON form
 * @returns {local var} gameConversions JSON file created with JSON.stringify that holds information matching request params
 * returns JSON file of each conversion in the slp file at location param (one big JSON file of multiple replays in future?)
 * in future, call a helper method according to what the request calls for? (depends on how much parsing we can do on the c# side and what a conversion json gives us)
 */
function getGameConversions(location) {
    var buffer = readFileSync(location); // reading file location into a buffer, THEN making a SlippiGame w/ the buffer is a workaround for JS not having same file access perms that C# does
    const game = new SlippiGame(buffer);
    const settings = game.getSettings();
    if (settings.isTeams == true) {
        console.log("doubles game passed to getGameConversions");
        // HACK: this currently causess an error if passed back through pipe B, but when implementing multiple games i can check before sending back through pipe B
        return "doubles game";
    } else {
        const stats = game.getStats();

        var gameConversions = {
            gameLocation: location,
            gameSettings: settings,
            conversionList: []
        };

        var conversions =
            stats === null || stats === void 0 ? void 0 : stats.conversions;
        conversions.forEach(function (conversion) {
            gameConversions.conversionList.push(addConversion(conversion, game, settings))
        })
        return gameConversions;
    }
}

/**
 * 
 * @param {ConversionType} conversion conversion from a given replay file that's being added to JSON sent through pipe B
 * @param {SlippiGame} game game the conversion belongs to
 * @param {GameStartType} settings settings from game the conversion belongs to
 * adds a given conversion in a replay to JSON file of all conversions in a replay
 */
function addConversion(conversion, game, settings) {
    const startFrameNum = conversion.startFrame;
    const endFrameNum = conversion.endFrame;

    const playerBeingHit = conversion.playerIndex;
    const pbhConnectCode = settings.players[playerBeingHit].connectCode;
    const playerHitting = conversion.lastHitBy;
    const phConnectCode = settings.players[playerHitting].connectCode;

    const frames = game.getFrames();

    // here we're setting up a JSON object that will eventually become an instance of the CSharpParser.SlpJSObjects.Conversion type
    var conversionFile = {
        playerBeingHit: playerBeingHit,
        beingHitConnectCode: pbhConnectCode,
        playerHitting: playerHitting,
        hittingConnectCode: phConnectCode,
        didKill: conversion.didKill,
        startPercent: conversion.startPercent,
        endPercent: conversion.endPercent,
        moves: conversion.moves,
        openingType: conversion.openingType,
        beingHitFrames: [],
        hittingFrames: []
    };

    for (
        var currentFrame = startFrameNum;
        currentFrame <= endFrameNum;
        currentFrame++
    ) {
        var beingHitFrame = frames[currentFrame].players[playerBeingHit].post;
        var hittingFrame = frames[currentFrame].players[playerHitting].post;
        conversionFile.beingHitFrames.push(beingHitFrame);
        conversionFile.hittingFrames.push(hittingFrame);
    }

    return conversionFile;
}

module.exports = {
    getGameConversions,
}