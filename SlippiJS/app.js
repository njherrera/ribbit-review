'use strict';

const { getGameConversions } = require("./src/RequestManager");
const { SlippiGame, conversions } = require("@slippi/slippi-js");
const { connectRequestPipe, createJsonPipe } = require("./src/PipeManager");
const fs  = require('fs');
/*
let data = JSON.stringify(getGameConversions("Q:\\programming\\ribbit-review\\.slp files\\EdgeguardTestSheikFalco.slp"));
fs.writeFile("Q:\\programming\\ribbit-review\\testJSONs\\EdgeguardSettingsTest.json", data, (err) => {
    if (err) throw err;
})*/

connectRequestPipe();