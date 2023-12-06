const { SlippiGame } = require("@slippi/slippi-js");

// move to separate class?

/** 
var conversionJSON = {
    playerIndexFrames: (Array),
    hitByFrames: (Array)
};

var conversionListJSON = {
    location: String,
    playerInfo: (Array),
    gameSettings: (Array),
    conversionList: (Array)
}
*/
/**
 * @param {string} location file location of replay that c# is requesting in JSON form
 * @returns {local var} gameConversions JSON file created with JSON.stringify that holds information matching request params
 * returns JSON file of each conversion in the slp file at location param (one big JSON file of multiple replays in future?)
 * in future, call a helper method according to what the request calls for? (depends on how much parsing we can do on the c# side and what a conversion json gives us)
 */
function getGameConversions(location) {
    const game = new SlippiGame(location);
    const stats = game.getStats();
    const settings = game.getSettings();

    var gameConversions = {
        gameLocation: location,
        gameSettings: [],
        conversionList: []
    };

    gameConversions.gameSettings.push(settings);

    var conversions =
        stats === null || stats === void 0 ? void 0 : stats.conversions;
    conversions.forEach(function (conversion) {
        gameConversions.conversionList.push(addConversion(conversion, game))
    })
    return gameConversions;
}

/**
 * 
 * @param {ConversionType} conversion conversion from a given replay file that's being added to JSON sent through pipe B
 * @param {SlippiGame} game game the conversion belongs to
 */
function addConversion(conversion, game) {
    const startFrameNum = conversion.startFrame;
    const endFrameNum = conversion.endFrame;

    const playerIndex = conversion.playerIndex;
    const hitByIndex = conversion.lastHitBy;

    const frames = game.getFrames();

    var conversionFile = {
        beingHitFrames: [],
        hittingFrames: []
    };

    for (
        var currentFrame = startFrameNum;
        currentFrame <= endFrameNum;
        currentFrame++
    ) {
        var beingHitFrame = frames[currentFrame].players[playerIndex].post;
        var hittingFrame = frames[currentFrame].players[hitByIndex].post;
        conversionFile.beingHitFrames.push(beingHitFrame);
        conversionFile.hittingFrames.push(hittingFrame);
    }

    return conversionFile;
}

module.exports = {
    getGameConversions,
}