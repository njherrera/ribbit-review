const { SlippiGame } = require("@slippi/slippi-js");
const { readFileSync } = require("fs");

/**
 * 
 * @param {string} filePaths comma-separated list of requested file paths 
 * @param {string} constraints string of key-value pairs w/ userId, userChar, oppChar, and stageId keys (GameSettings constraints)
 * @returns {allConversions} array of gameConversions objects, one for each requested location
 */


function getAllConversions(constraints, filePaths) {
    let allConversions = []

    const constraintsObject = createConstraintsObject(constraints);
    var constraintsJSON = JSON.stringify(constraintsObject);
    console.log(constraintsJSON.toString());

    let pathsArray = filePaths.split(",");
    for (let i = 0; i < pathsArray.length; i++) {
        let pathURL = new URL(pathsArray[i]);
        var pathConversions = getGameConversions(pathURL, constraintsObject);
        if (pathConversions != null) { // if pathConversions is null, it means that it was either a doubles replay or didn't match constraints
            allConversions.push(pathConversions);
        } else continue; 
    }
    return allConversions;
}

/** 
 * @param {string} constraints string of key-value pairs w/ userId, userChar, oppChar, and stageId keys (GameSettings constraints)
 * constraints string is formatted as such: "userId: userChar: oppChar: stageId:", w/ userId being connect code/in-game tag and others being in-game int IDs
 * @returns {gameConstraints} JSON object representing the key-value pairs of constraints
 */
function createConstraintsObject(constraints) {
    const indivConstraints = constraints.split(" ");

    var gameConstraints = {
    userId: "",
    userChar: "",
    oppChar: "",
    stageId: ""
    };

    const uId = indivConstraints[0].substring(indivConstraints[0].indexOf(":") + 1);
    const uChar = indivConstraints[1].substring(indivConstraints[1].indexOf(":") + 1);
    const oChar = indivConstraints[2].substring(indivConstraints[2].indexOf(":") + 1);
    const stage = indivConstraints[3].substring(indivConstraints[3].indexOf(":") + 1);

    gameConstraints.userId = uId;
    gameConstraints.userChar = uChar;
    gameConstraints.oppChar = oChar;
    gameConstraints.stageId = stage;
    return gameConstraints;
}
/**
 * 
 * @param {gameConstraints} constraints JSON object w/ GameSettings constraints - if no constraint for a given attribute, it will have a null value (i.e. userId: MMRP#834, userChar: 7, oppChar: 7, stageId: null for a specific matchup on any stage)
 * @param {GameStartType} settings GameSettings of a given .slp file
 * @returns {boolean} true if .slp file passes check, false if not
 */
function checkGameConstraints(constraints, settings) {
    // checking w/ separate if statements to make sure all relevant checks are executed
    if (settings.isTeams == true) {
        //console.log("doubles game passed to getGameConversions");
        return false;
    }
    if (constraints.userChar !== "") { 
        // if there's no player in the replay w/ a matching connect code/in-game tag, userPlayer = undefined
        // need to handle case where userPlayer = undefined by also returning false (similar thing w/ checking oppChar)
        const userPlayer = settings.players.find(element => element.connectCode.toString() === constraints.userId.toString());
        if (userPlayer == undefined) {
            //console.log("userPlayer undefined, returning false");
            return false;
        }
        let passesCheck = (parseInt(constraints.userChar) == userPlayer.characterId);
        if (passesCheck == false) {
            //console.log("user char does not match, returning false");
            return false;
        } // if int ID of user char does not match, return false - if true, continue with if statements
    }
    if (constraints.oppChar !== "") {
        const userPlayer = settings.players.find(element => element.connectCode.toString() === constraints.userId.toString());
        if (userPlayer == undefined) {
            //console.log("userPlayer undefined, returning false");
            return false;
        }

        const oPlayer = settings.players.find(element => element.connectCode.toString() != constraints.userId.toString());
        let passesCheck = (parseInt(constraints.oppChar) == oPlayer.characterId);
        if (passesCheck == false) {
            //console.log("opponent character does not match, returning false");
            return false;
        }
    }
    if (constraints.stageId !== "") {
        let passesCheck = (parseInt(constraints.stageId) == settings.stageId);
        if (passesCheck == false) {
            //console.log("stage does not match, returning false");
            return false;
        }
    }
    return true; // if we're here, it means that the replay hasn't returned false for any relevant checks and therefore matches constraints
}

/**
 * @param {string} location file URL of replay that c# is requesting 
 * @param {gameConstraints} constraints 
 * @returns {gameConversions} JSON object that holds all the conversions/game info for the replay at the location
 * @returns {null} if game is doubles or doesn't meet GameSettings constraints 
 * returning null so that we don't have to read the same replay into buffer twice
 */
function getGameConversions(location, constraints) {
    let buffer = readFileSync(location); // reading file location into a buffer, THEN making a SlippiGame w/ the buffer is a workaround for JS not having same file access perms that C# does
    const game = new SlippiGame(buffer);
    const settings = game.getSettings();

    if (checkGameConstraints(constraints, settings) == true) {
        //console.log("replay matches constraints");
        const stats = game.getStats();

        let gameConversions = {
            gameLocation: location,
            gameSettings: settings,
            conversionList: []
        };

        let conversions =
            stats === null || stats === void 0 ? void 0 : stats.conversions;
        conversions.forEach(function (conversion) {
            gameConversions.conversionList.push(addConversion(conversion, game, settings))
        })
        return gameConversions;
    } else {
        //console.log("replay does not match constraints");
        return null;
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

    for (var currentFrame = startFrameNum;
        currentFrame <= endFrameNum;
        currentFrame++)
    {
        let beingHitFrame = frames[currentFrame].players[playerBeingHit].post;
        let hittingFrame = frames[currentFrame].players[playerHitting].post;
        conversionFile.beingHitFrames.push(beingHitFrame);
        conversionFile.hittingFrames.push(hittingFrame);
    }

    return conversionFile;
}

module.exports = {
    getAllConversions,
}