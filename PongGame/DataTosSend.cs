using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
namespace PongGame
{
   public class DataTosSend
    {


        public int BallX;
        public int BallY;

        public DataTosSend Newdata;
       
        public void JsonWhriteData()
        {
            DataContractJsonSerializer Jsonf = new DataContractJsonSerializer(typeof(DataTosSend));

            using (FileStream fs = new FileStream("BallPosition.json", FileMode.OpenOrCreate))
            {
                Jsonf.WriteObject(fs, Game1.Instance.dataBall);
            }
        }

        public void readData()

        {
            DataContractJsonSerializer Jsonf = new DataContractJsonSerializer(typeof(DataTosSend));
            using (FileStream fs = new FileStream("BallPosition.json", FileMode.OpenOrCreate))
            {
                Newdata = (DataTosSend)Jsonf.ReadObject(fs);
            }
        }

    }
}
