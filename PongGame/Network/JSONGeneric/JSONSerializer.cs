using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Security.Cryptography;

using CryptoLibrary;

namespace PongGame.Network.JSONGeneric
{
    /// <summary>
    /// Generic json serializer that can distinguish between types of objects that has been serialized before deserializetion
    /// DOES NOT SUPPORT POLYMORPHISM YET !!! 
    /// </summary>
    public static class JSONSerializer
    {
        private const string PASSWORD = "password";
        private const string SALT = "salt";

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Serializes a data packet into a string using encoding ASCII
        /// </summary>
        /// <typeparam name="T">The type of object to serialize</typeparam>
        /// <param name="dataPacketToSerialize">The object to serialize</param>
        /// <param name="data">The serialized data as a string</param>
        /// <returns>True if the serialization is succefull, false otherwise</returns>
        public static bool SerializeData<T>(T dataPacketToSerialize, out string data)
        {
            return SerializeData(dataPacketToSerialize, out data, Encoding.ASCII);
        }

        /// <summary>
        /// Seirializes the object to a string using ASCII encoding
        /// </summary>
        /// <typeparam name="T">The type of object to serialize</typeparam>
        /// <param name="dataPacketToSerialize">The object to serialize</param>
        /// <param name="data">The serialize data in string form</param>
        /// <returns>True if the serializetion is successful, false otherwise</returns>
        public static bool SerializeData<T>(T dataPacketToSerialize, out string data, Encoding encoding)
        {
            data = null;

            MemoryStream memoryStream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            serializer.WriteObject(memoryStream, dataPacketToSerialize);

            // Repositions the reading starting point of the memory stream to the begining
            memoryStream.Position = 0;
            using (StreamReader streamReader = new StreamReader(memoryStream, encoding))
            {
                // formats the data to inculde the type of object it's serialized
                string classType = dataPacketToSerialize.GetType().ToString();

                // The encoding type used 
                string encodingType = encoding.EncodingName;

                // This should not be hardcoded into the deserializer and serializer 
                string unEncryptedData = string.Format("{0}:{1}:{2}", classType, encodingType, streamReader.ReadToEnd());

                data = CryptoHelper.Encrypt<TripleDESCryptoServiceProvider>(unEncryptedData, PASSWORD, SALT);
            }

            if (data != null)
            {
                // if the data isn't null returns true
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deserializes the string data back into the object
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize to</typeparam>
        /// <param name="data">The data to deserialize</param>
        /// <param name="dataPacket">The deserialize object</param>
        /// <returns>True if the deserializetion is successful, false otherwise</returns>
        public static bool DeSerializeData<T>(string data, out T dataPacket)
        {
            dataPacket = default(T);

            // This should not be hardcoded into the deserializer and serializer 
            string unDecryptedData = CryptoHelper.Decrypt<TripleDESCryptoServiceProvider>(data, PASSWORD, SALT);

            // Splits up the data into sections of a class type to decode, the encoding type and the concret data to deserialize
            if (SplitData(unDecryptedData, out string type, out Encoding encoding, out string dataPacketToDeSerialize))
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
        #endregion

        #region PRIVATE FUNCTIONS
        /// <summary>
        /// Splits the data from the serializetion into a type and data to deserialize
        /// </summary>
        /// <param name="data">The data to split</param>
        /// <param name="type">The type of object to deserialize</param>
        /// <param name="dataPacketToDeSerialize">The data that needs to be deserialize bag into an object</param>
        /// <returns>True if the spilt is successful, false otherwise</returns>
        private static bool SplitData(string data, out string classType, out Encoding encoding, out string dataPacketToDeSerialize)
        {
            classType = null;
            dataPacketToDeSerialize = null;
            encoding = null;

            if (data != null)
            {
                string[] splitData = data.Split(':');

                classType = splitData[0];

                if (GetEncoding(splitData[1], out Encoding enco))
                {
                    encoding = enco;
                }

                StringBuilder sb = new StringBuilder();

                for (int i = 2; i < splitData.Length; i++)
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

        /// <summary>
        /// Get the encoding from its name
        /// </summary>
        /// <param name="encodingName">The name of the encoding</param>
        /// <param name="encoding">The encoding that matches the name param</param>
        /// <returns>True if the encoding is found in the encoding info list, false otherwise</returns>
        private static bool GetEncoding(string encodingName, out Encoding encoding)
        {
            encoding = null;

            EncodingInfo[] encodingInfo = Encoding.GetEncodings();

            for (int i = 0; i < encodingInfo.Length; i++)
            {
                if (encodingInfo[i].DisplayName == encodingName)
                {
                    encoding = Encoding.GetEncoding(encodingInfo[i].Name);
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}