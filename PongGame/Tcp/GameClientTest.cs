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
        public static string ClientRecvieData;

        TcpDataPacket TcpDataPacket;

        string message;
        enum dataTosend
        {
            ballx,
            bally,
            padx,
            pady
        }

        dataTosend dataType = new dataTosend();
        
        enum DataRecived
        {
          PadX,
          pady
        }

        DataRecived recived;
      public void Connect(string sever)
        {
            dataType = dataTosend.ballx;
            recived = DataRecived.PadX;
            while (Game1.ImClient ==true)
            {
               
                DeBugString = "ClietnRuning";
                Thread.Sleep(1);

                try
                {
                    Int32 port = 13000;

                    TcpClient client = new TcpClient("127.0.0.1", port);/// sætter op tclpvlient med port nummer og id nummer
                    DeBugString = "write waht you wanne send\n";

                    switch (dataType)
                    {
                        case dataTosend.ballx:
                            message = Convert.ToString((int)Ball.BallPositioN.X);
                            dataType = dataTosend.bally;
                            break;
                        case dataTosend.bally:
                            message = Convert.ToString((int)Ball.BallPositioN.Y);
                            dataType = dataTosend.pady;
                            break;
                        case dataTosend.pady:
                            message = Convert.ToString((int)Pad.pad2Positiom.Y);
                            dataType = dataTosend.ballx;          
                            break;
                    }
                    // message = Convert.ToString((int)Ball.BallPositioN.Y);




                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);// laver massage om til byte
                    NetworkStream stream = client.GetStream();// sætter op stream
                    stream.Write(data, 0, data.Length);// skriver data til sever


                    DeBugString = "send data {0}" + message;
                    data = new byte[256];// byte array
                    string responsedata = string.Empty;// string for repondataet                               

                    int bytes = stream.Read(data, 0, data.Length); // reads data to byte

                    responsedata = System.Text.Encoding.ASCII.GetString(data, 0, bytes);// laver repsonse dat

                    Pad.pad1Position.Y = Convert.ToInt32(responsedata);
                    
                   ClientRecvieData = "resicved data: " + responsedata;
                   
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
                Game1.GameStart = true;
            }
            Game1.severStarted = false;
       

            DeBugString = "client Not Runnong";
        }

    }
}

