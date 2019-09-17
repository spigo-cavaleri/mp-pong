using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;


namespace PongGame
{
    [Serializable]
  public  class BallData
    {
        [DataMember]
        public int X =0;
        [DataMember]
        public int Y =0;


    }
}
