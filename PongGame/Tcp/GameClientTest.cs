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
namespace PongGame.Tcp
{
  public class GameClientTest
    {


     public static string DeBugString;

        TcpDataPacket TcpDataPacket;

        string message;
        enum dataTosend
        {
            ballx,
            bally
        }

        dataTosend dataType = new dataTosend();
        
      public void Connect(string sever)
        {
            dataType = dataTosend.bally;
            while (Game1.ImClient ==true)
            {
               
                DeBugString = "ClietnRuning";
                Thread.Sleep(8);

                try
                {
                    Int32 port = 13000;

                    TcpClient client = new TcpClient("127.0.0.1", port);/// sætter op tclpvlient med port nummer og id nummer
                    DeBugString = "write waht you wanne send\n";

                    switch (dataType)
                    {
                        //case dataTosend.ballx:
                        //     message = Convert.ToString((int)Ball.BallPositioN.X);
                        //    dataType = dataTosend.bally;
                        //    break;
                        case dataTosend.bally:
                            
                          //  dataType =dataTosend.ballx;

                            break;                       
                    }
                    message = Convert.ToString((int)Ball.BallPositioN.Y);




                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);// laver massage om til byte
                    NetworkStream stream = client.GetStream();// sætter op stream
                    stream.Write(data, 0, data.Length);// skriver data til sever
                    DeBugString = "send data {0}" + message;
                    data = new byte[256];// byte array
                    string responsedata = string.Empty;// string for repondataet
                    string moreresponsedata = null;
                      



                    int bytes = stream.Read(data, 0, data.Length);

                    responsedata = System.Text.Encoding.ASCII.GetString(data, 0, bytes);// laver repsonse dat

                    DeBugString = "resicved data {0}," + responsedata;
                    Game1.GameStart = true;
                    stream.Close();
                    client.Close();
                }
                catch (ArgumentNullException e)
                {
                    DeBugString = "argumentNUlleecpetion: {0}";
                }
                catch (SocketException e)
                {
                    Game1.ImClient = false;
                    
                    DeBugString = "socketecpetion";
                }

            }
            Game1.severStarted = false;
            
            DeBugString = "client Not Runnong";
        }

    }
}

