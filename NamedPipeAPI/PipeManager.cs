using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NamedPipeAPI
{
    public class PipeManager
    {
        private static string PIPE_A_NAME = "request_pipe";
        private static string PIPE_B_NAME = "json_pipe";

        public static void createRequestPipe()
        {
            var pipeServer = new NamedPipeServerStream(PIPE_A_NAME, PipeDirection.Out);

            Debug.WriteLine("pipe A waiting for connection");
            pipeServer.WaitForConnection();

            Debug.WriteLine("C# has connected to pipe A");
            try
            {
                using (StreamWriter sw = new StreamWriter(pipeServer))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("Q:\\programming\\ribbit-review\\.slp files\\EdgeguardTestSheikFalco"); // in future, this will be file location + request params
                    // in future, send message once all selected files have been requested
                    Debug.WriteLine("request to JS through pipe A has been written");
                }
            }
            catch (EndOfStreamException)
            {
                Debug.WriteLine("TS client disconnected, closing pipe A now");
            }
            catch (IOException e) // in case pipe is broken 
            {
                Debug.WriteLine("error in pipe A: ERROR: {0} ", e.Message);
            }
            /* perform closing and disposing upon exit of program/file selection tool - pipes are kept open while program is running (will need to verify that they close on exit)
            pipeServer.Close();
            pipeServer.Dispose();
            */
        }

        /* createPipeIn method (pipe D)
        * create client for pipe D (python > C#) with var pipeDClient = new NamedPipeClientStream("py_to_csharp", PipeDirection.In)
        * connect to existing pipe with pipeDClient.Connect()
        * make stream reader to read messages from pipe with pipeDsr = new StreamReader(pipeDClient)
        * receive JSON file(s) with sr.ReadLine (other method more friendly to JSON files? ReadToEnd?)
        * close pipe upon receiving "SHIT GET OUT" message
        */
        public static void connectJsonPipe()
        {
            using (var pipeClient = new NamedPipeClientStream(".", PIPE_B_NAME, PipeDirection.In))
            {

                Debug.WriteLine("C# attempting to connect to pipe B");
                pipeClient.Connect();

                Debug.WriteLine("C# has connected to pipe B");
                try
                {
                    using (StreamReader sr = new StreamReader(pipeClient))
                    {
                        string message = "";
                        if (sr.Peek() > 0)
                        {
                            message = sr.ReadLine();
                            while (message != null)
                            {
                                Debug.WriteLine(message);
                            }
                            
                        }
                    }
                }
                catch (EndOfStreamException)
                {
                    Debug.WriteLine("JS server disconnected, pipe B is closed");
                }
                catch (IOException e)
                {
                    Debug.WriteLine("error in pipe B: ERROR: {0} ", e.Message);
                }
            }
        }

        
    }
}
