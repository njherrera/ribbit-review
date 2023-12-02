'use strict';

const { handleConversionRequest } = require("./src/RequestManager");
const { SlippiGame, conversions } = require("@slippi/slippi-js");
const fs  = require('fs');

//console.log(handleConversionRequest("Q:\\programming\\ribbit-review\\.slp files\\EdgeguardTestSheikFalco.slp"));
var game = new SlippiGame("Q:\\programming\\ribbit-review\\.slp files\\EdgeguardTestSheikFalco.slp");
var stats = game.getStats();
var frames = game.getFrames();
var startFrameNum = stats.conversions[0].startFrame;
var endFrameNum = stats.conversions[0].endFrame;
var firstConversion = stats.conversions[0];
var playerIndex = firstConversion.playerIndex;
var hitByIndex = firstConversion.lastHitBy;
var startFrame = frames[startFrameNum].players[hitByIndex].post;

function conversionToJSON(conversion, game) {
    var startFrameNum = conversion.startFrame;
    var endFrameNum = conversion.endFrame;

    var playerIndex = conversion.playerIndex;
    var hitByIndex = conversion.lastHitBy;

    var frames = game.getFrames();

    var conversionFile = {
        pIndexFrames: [],
        hitByFrames: []
    };;

    for (
        var currentFrame = startFrameNum;
        currentFrame <= endFrameNum;
        currentFrame++
    ) {
        var pIndexFrame = frames[currentFrame].players[playerIndex].post;
        var hitByFrame = frames[currentFrame].players[hitByIndex].post;
        conversionFile.pIndexFrames.push(pIndexFrame);
        conversionFile.hitByFrames.push(hitByFrame);
    }

    return conversionFile;
}

//console.log(startFrame);
let data = JSON.stringify(conversionToJSON(firstConversion, game));
fs.writeFile("Q:\\programming\\ribbit-review\\test JSON files\\testJSON.txt", data, (err) => {
    if (err) throw err;
});