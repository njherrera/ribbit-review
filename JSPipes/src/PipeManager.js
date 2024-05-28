const net = require("net");
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
        //console.log("JS client: data received through pipe A:" + data.toString());
        let receivedPaths = data.toString();
        completePaths += receivedPaths;
    });

    requestClient.on('error', (err) => {
        if (err.message.indexOf('ENOENT') > -1) {
            setTimeout(() => {
                //console.log("JS client: attempting to re-connect");
                connectClient();
            }, 1000);
        }
        console.log(err.message);
    });

    requestClient.on('end', () => {
        //console.log("JS client: end event triggered");
        if (completePaths.length > 0) {
            //console.log("JS client: creating JS server");
            dataReceived = completePaths;
            //console.log("JS client: current requestedPaths value post-assignment: " + requestedPaths);
        }
    });

    requestClient.on('close', () => {
        //console.log("JS client: disconnected")
    });
}

/** pipe B (JS > c#)
 * passes a JSON file matching request from pipe A to c#
 */

function startServer() {
    const jsonServer = net.createServer(function (socket) {
        //console.log("JS server: C# client has connected to json pipe");
        socket.on('end', () => {
            //console.log("JS server: C# client has disconnected, restarting JS client now");
            connectClient();
        });
        // sending requested JSON data to C# client
        //console.log("JS server: calling getAllConversions using sendJsonData method");
        const splitRequest = dataReceived.split('|');
        const gameConstraints = splitRequest[0];
        const requestedPaths = splitRequest[1];
        let returnConversions = getAllConversions(gameConstraints, requestedPaths);
        let chunkSize = 5;
        let arrayOfCArrays = [];
        for (let i = 0; i < returnConversions.length; i += chunkSize) {
            arrayOfCArrays.push(returnConversions.slice(i, i + chunkSize));
        }
        arrayOfCArrays.forEach((array) => {
            let data = JSON.stringify(array);
            socket.write(data);
            console.log("JS Server: sent a batch of JSON objects through pipe");
        })
        socket.write("\n");
        socket.pipe(socket);
        //console.log("JS server: pipe executed successfully")
    });

    jsonServer.on('close', () => {
        //console.log("JS server: closed")
    }); 

    jsonServer.listen(PIPE_PATH + PIPE_B_NAME, () => {
        //console.log("JS server: now listening for a connection on pipe B");
    });
}

module.exports = {
    connectClient,
    startServer
}
