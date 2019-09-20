using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace PongGame.Tcp.JSONGeneric
{
    /// <summary>
    /// Generic json serializer that can distinguish between types of objects that has been serialized before deserializetion
    /// </summary>
    public static class JSONSerializer
    {
        /// <summary>
        /// Seirializes the object to a string using ASCII encoding
        /// </summary>
        /// <typeparam name="T">The type of object to serialize</typeparam>
        /// <param name="dataPacketToSerialize">The object to serialize</param>
        /// <param name="data">The serialize data in string form</param>
        /// <param name="encoding">The encoding used to serialize</param>
        /// <returns>True if the serializetion is successful, false otherwise</returns>
        public static bool SerializeData<T>(T dataPacketToSerialize, out string data, Encoding encoding)
        {   
            data = null;

            MemoryStream memoryStream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            //Writes the T object into the memory stream
            serializer.WriteObject(memoryStream, dataPacketToSerialize);

            // Repositions the reading starting point of the memory stream to the begining
            memoryStream.Position = 0;
            using (StreamReader streamReader = new StreamReader(memoryStream, encoding))
            {
                // formats the data to inculde the type of object it's serialized this is used at the deserializers end
                data = string.Format("{0}:{1}", dataPacketToSerialize.GetType().ToString(), streamReader.ReadToEnd());
            }

            if (data != null)
            {
                // if the data isn't null returns true
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deserializes the string data to the type of T object
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize to</typeparam>
        /// <param name="data">The data to deserialize</param>
        /// <param name="dataPacket">The deserialize object</param>
        /// <param name="encoding">The encoding used for deserializetion</param>
        /// <returns>True if the deserializetion is successful, false otherwise</returns>
        public static bool DeSerializeData<T>(string data, out T dataPacket, Encoding encoding)
        {
            dataPacket = default(T);

            // Splites the data string into a object type and the data the 
            if (SplitData(data, out string type, out string dataPacketToDeSerialize))
            {
                // Compares type with the T type object
                if (type == typeof(T).ToString())
                {
                    MemoryStream memoryStream = new MemoryStream(encoding.GetBytes(dataPacketToDeSerialize));
                    DataContractJsonSerializer deSerializer = new DataContractJsonSerializer(typeof(T));

                    dataPacket = (T)deSerializer.ReadObject(memoryStream);

                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// Splits the data from the serializetion into a type and data to deserialize
        /// </summary>
        /// <param name="data">The data to split</param>
        /// <param name="type">The type of object to deserialize</param>
        /// <param name="dataPacketToDeSerialize">The data that needs to be deserialize bag into an object</param>
        /// <returns>True if the spilt is successful, false otherwise</returns>
        private static bool SplitData(string data, out string type, out string dataPacketToDeSerialize)
        {
            type = null;
            dataPacketToDeSerialize = null;

            if (data != null)
            {
                string[] splitData = data.Split(':');

                type = splitData[0];

                StringBuilder sb = new StringBuilder();

                for (int i = 1; i < splitData.Length; i++)
                {
                    sb.Append(splitData[i]);

                    if (i < splitData.Length - 1)
                    {
                        sb.Append(":");
                    }
                }

                dataPacketToDeSerialize = sb.ToString();

                return true;
            }

            return false;
        }
    }
}
