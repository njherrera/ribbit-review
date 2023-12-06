using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

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

        /* createPipeIn method (pipe B)
         * in future, this will either stay open and read multiple JSONs, or read one big JSON that contains all of the info we need
        */
        public static string connectJsonPipe(){
            string message = "";
            using (var pipeClient = new NamedPipeClientStream(".", PIPE_B_NAME, PipeDirection.In))
            {

                Debug.WriteLine("C# attempting to connect to pipe B");
                pipeClient.Connect();

                Debug.WriteLine("C# has connected to pipe B");
                try
                {
                    using (StreamReader sr = new StreamReader(pipeClient))
                    {
                        if (sr.Peek() > 0)
                        {
                            message = sr.ReadLine();
                            while (message != null)
                            {
                                return message;
                            }
                        } else return message;
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
            return message;
        }

    }
}
