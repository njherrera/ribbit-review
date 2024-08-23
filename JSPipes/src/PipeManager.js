const net = require("net");
const fs = require("fs");
const JSONStream = require("JSONStream");
const { getAllConversions } = require("./RequestManager");

const PIPE_A_NAME = "request_pipe";
const PIPE_B_NAME = "json_pipe";
const PIPE_PATH = "\\\\.\\pipe\\";
var dataReceived = null;

/** pipe A (c# > JS)
 * receives a request for a JSON file from c#
 * in future will include file location(s) and possibly params for JSON
 */

function connectClient() {
    var requestClient = net.createConnection(PIPE_PATH + PIPE_A_NAME, () => {
        //console.log("JS client: connected to server!");
    });
    var completePaths = "";

    requestClient.on('data', (data) => {
        let receivedPaths = data.toString();
        completePaths += receivedPaths;
    });

    requestClient.on('error', (err) => {
        if (err.message.indexOf('ENOENT') > -1) {
            setTimeout(() => {
                connectClient();
            }, 1000);
        }
        console.log(err.message);
    });

    requestClient.on('end', () => {
        if (completePaths.length > 0) {
            dataReceived = completePaths;
        }
    });

    requestClient.on('close', () => {
    });
}

/** pipe B (JS > c#)
 * passes a JSON file matching request from pipe A to c#
 */

function startServer() {
    const jsonServer = net.createServer(function (socket) {
        socket.on('end', () => {
            connectClient();
        });
        // sending requested JSON data to C# client
        const splitRequest = dataReceived.split('|');
        const gameConstraints = splitRequest[0];
        const requestedPaths = splitRequest[1];
        let returnConversions = getAllConversions(gameConstraints, requestedPaths);

        let jsonStream = JSONStream.stringify("[", ",", "]\n");
        jsonStream.pipe(fileOutputStream);
        returnConversions.forEach(jsonStream.write);
        jsonStream.end();
    });

    jsonServer.on('close', () => {
        console.log("JS server: closed")
    }); 

    jsonServer.listen(PIPE_PATH + PIPE_B_NAME, () => {
    });
}

module.exports = {
    connectClient,
    startServer
}
