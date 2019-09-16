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
    [Serializable()]
    class Animel
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public double Height { get; set; }
        [DataMember]
        public int AnimalID { get; set; }


        public Animel()
        {

        }

        public Animel(string name = "No Name", double weight = 0, double height = 0)
        {
            Name = name;
            Weight = weight;
            Height = height;
        }

        public override string ToString()
        {

            return string.Format("{0} weight {1}, lbs and is {2} inches tall", Name, Weight, Height);

        }

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{

        //    info.AddValue("Name", Name);
        //    info.AddValue("Weight", Weight);
        //    info.AddValue("Height", Height);

        //}
        //public Animel(SerializationInfo info, StreamingContext context)
        //{
        //    Name = (string)info.GetValue("Name", typeof(string));
        //    Weight = (double)info.GetValue("Weight", typeof(double));
        //    Height = (double)info.GetValue("Height", typeof(double));
        //}
    }
}
