using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PongGame.GamePong;
using PongGame.Tcp;
using PongGame.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PongGame.Tcp
{ 
  public  class GameSeverTest
    {

        public static string TestDebugSever ="Sever not running";
        public static string DatagrotingfromClietn;
        public static Vector2 ballP;
        enum DataComingin
        {
            ballx,
            bally,
            
        }
        DataComingin DataComing = DataComingin.bally;
        public void StartSever()
        {
            

            while (true)
            {
               // Thread.Sleep(13);
                TcpListener server = null;
                try
                {
                    Int32 port = 13000;
                    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                    server = new TcpListener(localAddr, port);
                    server.Start();
                    Byte[] bytes = new Byte[256];
                    String data = null;
                    while (true)
                    {
                       TestDebugSever= "Waiting for a connection... ";
                        TcpClient client = server.AcceptTcpClient(); //her stopper koden
                        TestDebugSever ="Connected!";
                        data = null;
                        NetworkStream stream = client.GetStream();
                        int i;
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);


                             DatagrotingfromClietn ="Received: "+  ballP.Y;
                            data = data.ToUpper();
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                            stream.Write(msg, 0, msg.Length);
                            TestDebugSever = "Sent: {0}" + data;
                            ballP.Y = Convert.ToInt32(data);
                            switch (DataComing)
                            {
                                //case DataComingin.ballx:
                                //    ballP.X = Convert.ToInt32(data);
                                //    DataComing = DataComingin.bally;
                                //    break;
                                case DataComingin.bally:
                                //    ballP.Y = Convert.ToInt32(data);
                                   // DataComing = DataComingin.ballx;
                                    break;

                            }
                        

                        }
                        client.Close();
                    }
                }
                catch (SocketException e)
                {
                    TestDebugSever = "SocketException: {0}" + e;
                }
                finally
                {
                    server.Stop();
                }
            }
        }
    }
}
