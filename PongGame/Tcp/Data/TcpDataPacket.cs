using PongGame.Tcp;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PongGame.Data
{

    public class TcpDataPacket
    {
        public readonly string Data;
        public readonly GameClient Client;

        private string data;


        public TcpDataPacket() { }
        public TcpDataPacket(GameClient client, string data)
        {
        
            Client = client;
            Data = data;
        }

        public string JsonWhriteData<T>(T gameData)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Position = 0;
            DataContractJsonSerializer Jsonf = new DataContractJsonSerializer(typeof(T));
            Jsonf.WriteObject(memoryStream, gameData);
            using (StreamReader reader = new StreamReader(memoryStream, Encoding.ASCII))
            {
                data = reader.ReadToEnd();
            }
            return data;
        }

        public T JsonReadData<T>()
        {
            T gameData = default(T);
            if (data != null)
            {
                MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(data));
                DataContractJsonSerializer Jsonf = new DataContractJsonSerializer(typeof(T));
                gameData = (T)Jsonf.ReadObject(memoryStream);

            }
            return gameData;
        }
    }
}
    