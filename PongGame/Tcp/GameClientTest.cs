using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace PongGame.Tcp
{
  public  class GameClientTest
    {

     public static string DeBugString;


      public bool Connect(string sever, string message)
        {

            bool won = false;
            try
            {
                Int32 port = 13000;

                TcpClient client = new TcpClient(sever, port);/// sætter op tclpvlient med port nummer og id nummer
                DeBugString ="write waht you wanne send\n";
                message = Console.ReadLine();

                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);// laver massage om til byte
                NetworkStream stream = client.GetStream();// sætter op stream
                stream.Write(data, 0, data.Length);// skriver data til sever
               DeBugString=  "send data {0}"+  message;
                data = new byte[256];// byte array
                string responsedata = string.Empty;// string for repondataet
                string moreresponsedata = null;

                int bytes = stream.Read(data, 0, data.Length);

                responsedata = System.Text.Encoding.ASCII.GetString(data, 0, bytes);// laver repsonse dat

               DeBugString = "resicved data {0}," + responsedata;

                if (responsedata == "gg")
                {
                    DeBugString = "you won!!!!!!!!!!!";

                    Console.ReadLine();
                    won = true;
                }
                stream.Close();
                client.Close();

            }
            catch (ArgumentNullException e)
            {
                //Console.WriteLine("argumentNUlleecpetion: {0}", e);
            }
            catch (SocketException e)
            {
             //   Console.WriteLine("socketecpetion");

            }

            return won;
        }

    }
}

