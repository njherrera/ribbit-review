'use strict';

const { getGameConversions } = require("./src/RequestManager");
const { SlippiGame, conversions } = require("@slippi/slippi-js");
const fs  = require('fs');

//console.log(handleConversionRequest("Q:\\programming\\ribbit-review\\.slp files\\EdgeguardTestSheikFalco.slp"));
/*var game = new SlippiGame("Q:\\programming\\ribbit-review\\.slp files\\EdgeguardTestSheikFalco.slp");
var stats = game.getStats();
var frames = game.getFrames();
var startFrameNum = stats.conversions[0].startFrame;
var endFrameNum = stats.conversions[0].endFrame;
var firstConversion = stats.conversions[0];
var playerIndex = firstConversion.playerIndex;
var hitByIndex = firstConversion.lastHitBy;
var startFrame = frames[startFrameNum].players[hitByIndex].post;

let data = JSON.stringify(conversionToJSON(firstConversion, game));
fs.writeFile("Q:\\programming\\ribbit-review\\test JSON files\\testJSON.txt", data, (err) => {
    if (err) throw err;
});*/

let data = JSON.stringify(getGameConversions("Q:\\programming\\ribbit-review\\.slp files\\EdgeguardTestSheikFalco.slp"));
fs.writeFile("Q:\\programming\\ribbit-review\\testJSONs\\testJSON.txt", data, (err) => {
    if (err) throw err;
})