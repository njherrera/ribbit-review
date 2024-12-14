const { SlippiGame } = require("@slippi/slippi-js");
const { readFileSync } = require("fs");

/**
 * 
 * @param {string} filePaths comma-separated list of requested file paths 
 * @param {string} constraints string of key-value pairs w/ userId, userChar, oppChar, and stageId keys (GameSettings constraints)
 * @returns {allConversions} array of gameConversions objects, one for each requested location
 */


function getAllConversions(constraints, paths) {
    let allConversions = []

    const constraintsObject = createConstraintsObject(constraints);

    let pathsArray = paths.split(",");
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
    stageId: "",
    isLocal: ""
    };

    const uId = indivConstraints[0].substring(indivConstraints[0].indexOf(":") + 1);
    const uChar = indivConstraints[1].substring(indivConstraints[1].indexOf(":") + 1);
    const oChar = indivConstraints[2].substring(indivConstraints[2].indexOf(":") + 1);
    const stage = indivConstraints[3].substring(indivConstraints[3].indexOf(":") + 1);
    const local = indivConstraints[4].substring(indivConstraints[4].indexOf(":") + 1);

    gameConstraints.userId = uId;
    gameConstraints.userChar = uChar;
    gameConstraints.oppChar = oChar;
    gameConstraints.stageId = stage;
    gameConstraints.isLocal = local;
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
        return false;
    }
    if (constraints.userId != "" && constraints.isLocal.toString() === "False") {
        const userPlayer = settings.players.find(element => element.connectCode.toString() === constraints.userId.toString());
        if (userPlayer == undefined) {
            return false;
        }
    }
    if (constraints.userId != "" && constraints.isLocal.toString() === "True") {
        const userPlayer = settings.players.find(element => element.nametag.toString() === constraints.userId.toString());
        if (userPlayer == undefined) {
            return false;
        }
    }
    if (constraints.userChar != "") { 
        // if there's no player in the replay w/ a matching connect code/in-game tag, userPlayer = undefined
        // need to handle case where userPlayer = undefined by also returning false (similar thing w/ checking oppChar)
        const userPlayer = settings.players.find(element =>
            (element.connectCode.toString() === constraints.userId.toString()) || (element.nametag.toString() === constraints.userId.toString()));
        if (userPlayer == undefined) {
            return false;
        }
        let passesCheck = (parseInt(constraints.userChar) == userPlayer.characterId);
        if (passesCheck == false) {
            return false;
        } // if int ID of user char does not match, return false - if true, continue with if statements
    }
    if (constraints.oppChar !== "") {
        const userPlayer = settings.players.find(element =>
            (element.connectCode.toString() === constraints.userId.toString()) || (element.nametag.toString() === constraints.userId.toString()));
        if (userPlayer == undefined) {
            return false;
        }

        const oPlayer = settings.players.find(element =>
            ((element.connectCode.toString() != constraints.userId.toString()) && element.connectCode != "")
            || element.nametag.toString() != constraints.userId.toString());
        let passesCheck = (parseInt(constraints.oppChar) == oPlayer.characterId);
        if (passesCheck == false) {
            return false;
        }
    }
    if (constraints.stageId !== "") {
        let passesCheck = (parseInt(constraints.stageId) == settings.stageId);
        if (passesCheck == false) { 
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
        console.log("replay matches constraints");
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
        console.log("replay does not match constraints");
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

    const victim = conversion.playerIndex;
    const attacker = conversion.lastHitBy;
    var victimPlayersIndex;
    var attackerPlayersIndex;

    // for games played offline, player index could be > 1 but player with lower player index will always correspond to the 0-index element in the settings.players[] array
    // therefore to make sure we're accessing the correct settings.Players[], we compare the indices of each player 
    // games played on slippi will always have players in ports 1 (index 0) and 2 (index 1), but offline games could have players in ports 3 (index 2) and/or 4 (index 3)
    if (attacker > victim) {
        attackerPlayersIndex = 1;
        victimPlayersIndex = 0;
    } else {
        attackerPlayersIndex = 0;
        victimPlayersIndex = 1;
    }

    const victimCode = settings.players[victimPlayersIndex].connectCode;
    const attackerCode = settings.players[attackerPlayersIndex].connectCode;

    const frames = game.getFrames();

    // here we're setting up a JSON object that will eventually become an instance of the CSharpParser.SlpJSObjects.Conversion type
    var conversionFile = {
        victimIndex: victim,
        victimConnectCode: victimCode,
        victimNametag: settings.players[victimPlayersIndex].nametag,
        attackerIndex: attacker,
        attackerConnectCode: attackerCode,
        attackerNametag: settings.players[attackerPlayersIndex].nametag,
        didKill: conversion.didKill,
        startPercent: conversion.startPercent,
        endPercent: conversion.endPercent,
        moves: conversion.moves,
        openingType: conversion.openingType,
        victimFrames: [],
        attackerFrames: []
    };

    for (var currentFrame = startFrameNum;
        currentFrame <= endFrameNum;
        currentFrame++)
    {
        let victimFrame = frames[currentFrame].players[victim].post;
        let attackerFrame = frames[currentFrame].players[attacker].post;
        conversionFile.victimFrames.push(victimFrame);
        conversionFile.attackerFrames.push(attackerFrame);
    }

    return conversionFile;
}

module.exports = {
    getGameConversions: function (callback, location) {
        let buffer = readFileSync(location); // reading file location into a buffer, THEN making a SlippiGame w/ the buffer is a workaround for JS not having same file access perms that C# does
        const game = new SlippiGame(buffer);
        const settings = game.getSettings();
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
        callback(null, gameConversions);
    }, 
    getAllConversions: function (callback, constraints, paths) {
        let allConversions = [];

        const constraintsObject = createConstraintsObject(constraints);

        let pathsArray = paths.split(",");
        for (let i = 0; i < pathsArray.length; i++) {
            let pathURL = new URL(pathsArray[i]);
            var pathConversions = getGameConversions(pathURL, constraintsObject);
            if (pathConversions != null) { // if pathConversions is null, it means that it was either a doubles replay or didn't match constraints
                allConversions.push(pathConversions);
            } else continue;
        }
        callback(null, allConversions);
    }
}