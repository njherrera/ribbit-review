const net = require("net");
const { getGameConversions } = require("./RequestManager");

let PIPE_A_NAME = "request_pipe";
let PIPE_B_NAME = "json_pipe";
let PIPE_PATH = "\\\\.\\pipe\\";

/** pipe A (c# > TS)
 * receives a request for a JSON file from c#
 * in future will include file location(s) and possibly params for JSON
 */

function connectRequestPipe() {
    const requestClient = net.createConnection(PIPE_PATH + PIPE_A_NAME, () => {
        console.log("JS connected to request pipe!");
    });

    requestClient.on('data', (data) => {
        console.log("data received through pipe A: " + data.toString());
        var requestData = data.toString();
        return requestData;
    });

    requestClient.on('end', () => {
        console.log("JS disconnected from request pipe")
    });
}

/** pipe B (TS > c#)
 * passes a JSON file matching request from pipe A to c#
 */

function createJsonPipe() {
    const jsonServer = net.createServer((c) => {
        console.log("C# client has connected to json pipe");
        c.on('end', () => {
            console.log("C# client has disconnected from json pipe")
        });
        let data = JSON.stringify(getGameConversions("Q:\\programming\\ribbit-review\\.slp files\\EdgeguardTestSheikFalco.slp"));
        c.write(data + "\n");
        c.pipe(c);
    });

    jsonServer.on('error', (err) => {
        throw err;
    });

    jsonServer.listen(PIPE_PATH + PIPE_B_NAME, () => {
        console.log("JS json server is now listening for a connection on pipe B");
    });
}

module.exports = {
    connectRequestPipe,
    createJsonPipe
}