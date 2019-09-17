using PongGame.Tcp;

namespace PongGame.Data
{

    public struct TcpDataPacket
    {
        public readonly string Data;
        public readonly GameClient Client;

        public TcpDataPacket(GameClient client, string data)
        {
            Client = client;
            Data = data;
        }
    }
}
