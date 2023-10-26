import win32pipe, win32file, pywintypes

# pipe server/creation

# connects to pipe A (C# > python) to read REQUEST from
# uses win32file.CreateFile to connect to r'\\.\pipe\read_request' pipe 
# calls win32file.ReadFile on the data returned by CreateFile (i.e. data = win32file.CreateFile(......))
# verifies that something of format <0, data> is returned by ReadFile
# sends data to request handler
requestHandle = win32file.CreateFile("\\\\.\\pipe\\read_request",
                              win32file.GENERIC_READ | win32file.GENERIC_WRITE,
                              0, None,
                              win32file.OPEN_EXISTING,
                              0, None)
requestData = win32file.ReadFile(requestHandle, 4096)
if requestData[0] == 0:
    print(requestData[1])
    # send data to RequestHandler
else: 
    print('ERROR', requestData[0])

# creates pipe B (python > TS) to write REQUEST into
# uses win32pipe.CreateNamedPipe to create r'\\.\pipe\write_request' pipe
# uses win32pipe.ConnectNamedPipe to connect pipe to TS
# invokes RequestHandler to format/verify the request
# uses win32file.WriteFile to send request through pipe B
writeRequestPipe = win32pipe.CreateNamedPipe(r'\\.\pipe\write_request',
    win32pipe.PIPE_ACCESS_DUPLEX,
    win32pipe.PIPE_TYPE_MESSAGE | win32pipe.PIPE_WAIT,
    1, 65536, 65536,300,None)

win32pipe.ConnectNamedPipe(writeRequestPipe, None)


requestData = "Forwarding request for JSON to TS"  
# invoke RequestHandler on requestData here, then send formatted/verified data through pipe
win32file.WriteFile(writeRequestPipe, requestData)


# connects to pipe C (TS > python) to read JSON from
# uses win32file.CreateFile to connect to r'\\.\pipe\read_json' pipe
# calls win32file.ReadFile on the data returned by CreateFile (i.e. data = win32file.CreateFile(......))
# verifies that something of format <0, data> is returned by ReadFile
# sends data to JSONHandler
fileHandle = win32file.CreateFile("\\\\.\\pipe\\read_json",
                              win32file.GENERIC_READ | win32file.GENERIC_WRITE,
                              0, None,
                              win32file.OPEN_EXISTING,
                              0, None)
jsonData = win32file.ReadFile(fileHandle, 4096)
if jsonData[0] == 0:
    print(jsonData[1])
    # send data to JSONHandler here
else:
    print('ERROR', jsonData[0])


# creates pipe D (python > C#) to write JSON into
# uses win32pipe.CreateNamedPIpe to create r'\\.\pipe\write_json' pipe
# uses win32.ConnectNamedPipe to connect pipe to C#
# invokes JSONHandler to format/verify the JSON object
# uses win32file.WriteFile to send JSON through pipe D
writeJsonPipe = win32pipe.CreateNamedPipe(r'\\.\pipe\write_json',
    win32pipe.PIPE_ACCESS_DUPLEX,
    win32pipe.PIPE_TYPE_MESSAGE | win32pipe.PIPE_WAIT,
    1, 65536, 65536,300,None)

win32pipe.ConnectNamedPipe(writeJsonPipe, None)


jsonData = "Forwarding JSON data to C#"  
# invoke JSONHandler on jsonData here, then send formatted/verified data through pipe 
win32file.WriteFile(writeJsonPipe, jsonData)