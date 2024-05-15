using System.IO.Pipes;
using System.Diagnostics;
using System.Text;

namespace NamedPipeAPI
{
    public class PipeManager
    {
        private static string PIPE_A_NAME = "request_pipe";
        private static string PIPE_B_NAME = "json_pipe";
        private static NamedPipeServerStream _pipeServerStream;
        private static NamedPipeClientStream _pipeClientStream;

        /* pipe A (c# > JS)
         * createRequestPipe creates a named pipe to send requests for .slp files through
        */
        public static void  openRequestPipe()
        {
            _pipeServerStream = new NamedPipeServerStream(PIPE_A_NAME, PipeDirection.Out);
            //Debug.WriteLine("C# server: waiting for connection");
            _pipeServerStream.WaitForConnection();

            //Debug.WriteLine("C# server: client has connected");
        }

        // call this upon exit of program
        public static void closeRequestPipe()
        {
            _pipeServerStream.Close();
        }


        public static void sendRequest(string filePaths)
        {
            Debug.WriteLine(filePaths.ToString());
            try
            {
                using (StreamWriter sw = new StreamWriter(_pipeServerStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine(filePaths);
                    //Debug.WriteLine("request has been sent through pipe A");
                    sw.Close();
                }
             }            
            catch (EndOfStreamException)
            {
                //Debug.WriteLine("TS client disconnected, closing pipe A server");
            }
            catch (IOException e) // in case pipe is broken 
            {
                //Debug.WriteLine("error in pipe A: ERROR: {0} ", e.Message);
            }
        }

        /* pipe B (JS > c#), receives a JSON file matching request from JS
        */
        // TODO: keep open until each group of JSON files has come through
        public static string readJson()
        {
            _pipeClientStream = new NamedPipeClientStream(".", PIPE_B_NAME, PipeDirection.In);

            //Debug.WriteLine("C# client: attempting connection");
            _pipeClientStream.Connect();
            //Debug.WriteLine("C# client: connected, reading JSON now");

                try
                {
                    using (StreamReader sr = new StreamReader(_pipeClientStream))
                    {
                    if (sr.Peek() > 0)
                    {
                        //Debug.WriteLine("C# client: sr.Peek > 0, reading data from pipe now");
                        string message;
                        while ((message = sr.ReadLine()) != null)
                        {
                            //Debug.WriteLine("C# client: received message from server");
                            sr.Close();
                            return message;
                        }
                    }
                    else
                    {
                        //Debug.WriteLine("C# client: sr.Peek < 0, nothing in pipe to read");
                        return "";
                    }
                    }
                }
                catch (EndOfStreamException)
                {
                    //Debug.WriteLine("C# client: JS server disconnected, pipe B is closed");
                }
                catch (IOException e)
                {
                    //Debug.WriteLine("error in pipe B: ERROR: {0} ", e.Message);
                }

            // can return null if stream is read when there's nothing in it
            return "";
        }

    }
}
