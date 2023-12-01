const { net } = require("net");

let PIPE_A_NAME = "request_pipe";
let PIPE_B_NAME = "json_pipe";
let PIPE_PATH = "\\\\.\\pipe\\";
// initalize name for pipe A as let PIPE_B_NAME = 'request_pipe'
// initialize name for pipe B as let PIPE_C_NAME = 'json_pipe'
// initalize pipe path prefix as let PIPE_PATH = '\\\\.\\pipe'

/** pipe A (c# > TS)
 * returns the string requestData, contains info on the request coming through the pipe from C#
 */

export function connectRequestPipe() {
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
 * modify to include input param (for passing json)
 */

export function createJsonPipe() {
    const jsonServer = net.createServer((c) => {
        console.log("C# client has connected to json pipe");
        c.on('end', () => {
            console.log("C# client has disconnected from json pipe")
        });
        c.write('hello this is JS writing through pipe B, this will be a JSON file in the future :3\n');
        c.pipe(c);
    });

    jsonServer.on('error', (err) => {
        throw err;
    });

    jsonServer.listen(PIPE_PATH + PIPE_B_NAME, () => {
        console.log("JS json server is now listening for a connection on pipe B");
    });
}