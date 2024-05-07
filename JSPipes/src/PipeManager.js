const net = require("net");
const { getAllConversions } = require("./RequestManager");

const PIPE_A_NAME = "request_pipe";
const PIPE_B_NAME = "json_pipe";
const PIPE_PATH = "\\\\.\\pipe\\";
var requestedPaths = null;

/** pipe A (c# > JS)
 * receives a request for a JSON file from c#
 * in future will include file location(s) and possibly params for JSON
 */

function connectClient() {
    var requestClient = net.createConnection(PIPE_PATH + PIPE_A_NAME, () => {
        console.log("JS client: connected to server!");
    });
    var completePaths = "";

    requestClient.on('data', (data) => {
        console.log("JS client: data received through pipe A:" + data.toString());
        let receivedPaths = data.toString();
        completePaths += receivedPaths;
    });

    requestClient.on('error', (err) => {
        if (err.message.indexOf('ENOENT') > -1) {
            setTimeout(() => {
                console.log("JS client: attempting to re-connect");
                connectClient();
            }, 1000);
        }
        console.log(err.message);
    });

    requestClient.on('end', () => {
        console.log("JS client: end event triggered");
        if (completePaths.length > 0) {
            console.log("JS client: creating JS server");
            requestedPaths = completePaths;
            console.log("JS client: current requestedPaths value post-assignment: " + requestedPaths);
        }
    });

    requestClient.on('close', () => {
        console.log("JS client: disconnected")
    });
}

/** pipe B (JS > c#)
 * passes a JSON file matching request from pipe A to c#
 */

function startServer() {
    const jsonServer = net.createServer(function (socket) {
        console.log("JS server: C# client has connected to json pipe");
        socket.on('end', () => {
            console.log("JS server: C# client has disconnected, restarting JS client now");
            connectClient();
        });
        // sending requested JSON data to C# client
        console.log("JS server: calling getAllConversions using sendJsonData method");
        let returnConversions = getAllConversions(requestedPaths);
        console.log("allConversions length: " + returnConversions.length)
        let data = JSON.stringify(returnConversions);
        console.log("JS server: called getAllConversions, writing data through pipe");
        socket.write(data + "\n");
        console.log("JS server: write executed successfully")
        socket.pipe(socket);
        console.log("JS server: pipe executed successfully")
    });

    jsonServer.on('close', () => {
        console.log("JS server: closed")
    });

    jsonServer.listen(PIPE_PATH + PIPE_B_NAME, () => {
        console.log("JS server: now listening for a connection on pipe B");
    });
}

module.exports = {
    connectClient,
    startServer
}


/*

let PIPE_A_NAME = "request_pipe";
let PIPE_B_NAME = "json_pipe";
let PIPE_PATH = "\\\\.\\pipe\\";


// TODO: keep pipes open until program is closed

*//** pipe A (c# > JS)
 * receives a request for a JSON file from c#
 * in future will include file location(s) and possibly params for JSON
 *//*

function connectRequestPipe() {
    var requestClient = net.createConnection(PIPE_PATH + PIPE_A_NAME, () => {
        console.log("JS connected to request pipe!");
    });
    var completeData = "";

    requestClient.on('data', (data) => {
        console.log("data received through pipe A:" + data.toString());
        let received = data.toString();
        completeData += received;
    });
*//*        if (data.toString().valueOf() == new String("FINISHED").valueOf())
        {
            console.log("FINISHED received through pipe A")
            let filePaths = receivedData;
            receivedData = ""; // doing it this way so that we don't go back into connectRequestPipe to reset the string after calling createJsonPipe
            createJsonPipe(filePaths);
        }
        else
        {
            console.log("data received through pipe A:" + data.toString());
            receivedData += data.toString();
        }*//*

    requestClient.on('error', (err) => {
        if (err.message.indexOf('ENOENT') > -1) {
            setTimeout(() => {
                console.log("pipe A client attempting to re-connect");
                connectRequestPipe();
            }, 1000);
        }
        console.log(err.message);
    });

    requestClient.on('end', () => {
        console.log("pipe A client end event triggered");
        if (completeData.length > 0) {
            console.log("creating pipe B server");
            createJsonPipe(completeData);
        }
    });

    requestClient.on('close', () => {
        console.log("JS pipe A client disconnected")
    });
}

*//** pipe B (JS > c#)
 * passes a JSON file matching request from pipe A to c#
 *//*
// TODO: set allowHalfOpen to "true" so pipe doesn't automatically close after writing data
function createJsonPipe(filePaths) {
    const jsonServer = net.createServer((c) => {
        console.log("C# client has connected to json pipe");
        c.on('end', () => {
            console.log("C# pipe B client has disconnected from server, connecting pipe A client now");
            //connectRequestPipe(); // re-connecting pipe A in case the user requests more replays
        });
        // sending JSON of GameConversions objects back through the pipe
        let returnJSON = JSON.stringify(getAllConversions(filePaths));
        let data = returnJSON;
        console.log("JS server: called getAllConversions, writing data through pipe");
        c.write(data + "\n");
    });

    jsonServer.on('error', (err) => {
        var errorMessage = err.message;
        console.log("error from pipe B server:" + errorMessage);
    });

    jsonServer.listen(PIPE_PATH + PIPE_B_NAME, () => {
        console.log("pipe B server now listening for a connection");
    });
}

module.exports = {
    connectRequestPipe,
    createJsonPipe
}*/