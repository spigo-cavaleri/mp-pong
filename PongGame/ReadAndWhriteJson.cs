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
   public class ReadAndWhriteJson
    {


       public GameData NewgameData = new GameData();
       
       
        public void JsonWhriteData()
        {
            DataContractJsonSerializer Jsonf = new DataContractJsonSerializer(typeof(GameData));

            using (FileStream fs = new FileStream("BallPosition.json", FileMode.Create))
            {
                Jsonf.WriteObject(fs, Game1.Instance.GameData);
            }
        }
      
        public void ReadDataJson()
        {
       
            DataContractJsonSerializer Jsonf = new DataContractJsonSerializer(typeof(GameData));
            using (FileStream fs = new FileStream("BallPosition.json", FileMode.Open))
            {
             NewgameData  = (GameData)Jsonf.ReadObject(fs);
            }
            
        }

    
    }
}
