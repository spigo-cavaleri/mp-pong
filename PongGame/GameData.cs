using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PongGame
{
    [Serializable()]
    public struct GameData
    {
        [DataMember]
        public int BallYPosition;
        [DataMember]
        public int BallXPosition;
        [DataMember]
        public int ModstandersPositionX;
        [DataMember]
        public int ModstandersPositionY;
        [DataMember]
        public int MypositionX;
        [DataMember]
        public int MyPositionY;
        [DataMember]
        public int MyHp;
        [DataMember]
        public int ModstandersHP;

        

    }
}
