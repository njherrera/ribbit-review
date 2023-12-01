'use strict';

const { handleConversionRequest } = require("./src/RequestManager");
const { SlippiGame } = require("@slippi/slippi-js");

//console.log(handleConversionRequest("Q:\\programming\\ribbit-review\\.slp files\\EdgeguardTestSheikFalco.slp"));
var game = new SlippiGame("Q:\\programming\\ribbit-review\\.slp files\\EdgeguardTestSheikFalco.slp");
var stats = game.getStats();
console.log(stats.conversions);
