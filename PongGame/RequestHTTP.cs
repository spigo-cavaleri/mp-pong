using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace PongGame
{
    class RequestHTTP
    {
        int paraLul = 0;

        public RequestHTTP()
        {

        }

        public void DoMagic()
        {
            WebRequest request = WebRequest.Create("https://my-json-server.typicode.com/typicode/demo/comments");
            request.Method = "POST";

            string postData = "This is a test that posts this string to a server";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
        }
    }
}
