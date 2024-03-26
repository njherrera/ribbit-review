const net = require("net");
const { getAllConversions } = require("./RequestManager");
const { existsSync } = require("fs");

let PIPE_A_NAME = "request_pipe";
let PIPE_B_NAME = "json_pipe";
let PIPE_PATH = "\\\\.\\pipe\\";

/** pipe A (c# > JS)
 * receives a request for a JSON file from c#
 * in future will include file location(s) and possibly params for JSON
 */

function connectRequestPipe() {
    const requestClient = net.createConnection(PIPE_PATH + PIPE_A_NAME, () => {
        console.log("JS connected to request pipe!");
    });

    requestClient.on('data', (data) => {
        console.log("data received through pipe A: " + data.toString());
        var requestedPaths = data.toString();
        createJsonPipe(requestedPaths);
    });

    requestClient.on('error', (err) => {
        if (err.message.indexOf('ENOENT') > -1) {
            setTimeout(() => {
                connectRequestPipe();
            }, 1000);
        }
    });

    requestClient.on('close', () => {
        console.log("JS pipe A client disconnected")
    });
}

/** pipe B (JS > c#)
 * passes a JSON file matching request from pipe A to c#
 */

function createJsonPipe(filePaths) {
    const jsonServer = net.createServer((c) => {
        console.log("C# client has connected to json pipe");
        c.on('end', () => {
            console.log("C# client has disconnected from json pipe")
        });
        // sending JSON of GameConversions objects back through the pipe
        let data = JSON.stringify(getAllConversions(filePaths));
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