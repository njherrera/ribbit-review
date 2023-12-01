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
 * @param {string} params signify what c# wants returned to it in JSON form
 * @returns {string} JSON file created with JSON.stringify that holds information matching request params
 * returns JSON file of each conversion in the slp file at location param (one big JSON file of multiple replays in future?)
 * in future, call a helper method according to what the request calls for? (depends on how much parsing we can do on the c# side and what a conversion json gives us)
 */
function handleConversionRequest(location) {
    var game = new SlippiGame(location);
    var stats = game.getStats();

    var returnJSON = {
        location: String,
        playerInfo: [],
        gameSettings: [],
        conversionList: []
    };
    var conversions =
        stats === null || stats === void 0 ? void 0 : stats.conversions;
    conversions.forEach(function (conversion) {
        addConversionToJSON(conversion, game, returnJSON)
    })
    return jsonFile;
}

/**
 * 
 * @param {ConversionType} conversion conversion from a given replay file that's being added to JSON sent through pipe B
 * @param {conversionsJSON} outputJSON JSON being sent through pipe B
 * @param {SlippiGame} game game the conversion belongs to
 */
function addConversionToJSON(conversion, game, outputJSON) {
    var firstConversionFrame = conversion.startFrame;
    var lastConversionFrame = conversion.endFrame;

    var playerIndex = conversion.playerIndex;
    var hitByIndex = conversion.hitByIndex;

    var frames = game.getFrames();

    var conversionFile = {
        playerIndexFrames: [],
        hitByFrames: []
    };;

    for (
        var currentFrame = firstConversionFrame;
        currentFrame <= lastConversionFrame;
        currentFrame++
    ) {
        conversionFile.playerIndexFrames.push(frames[currentFrame].players[playerIndex].post);
        conversionFile.hitByFrames.push(frames[currentFrame].players[hitByIndex].post);
    }

    outputJSON.conversionList.push(conversionFile);
}

module.exports = {
    handleConversionRequest,
}