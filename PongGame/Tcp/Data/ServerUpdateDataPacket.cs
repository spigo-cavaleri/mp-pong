using PongGame.GamePong;

namespace PongGame.Tcp.Data
{
    public struct ServerUpdateDataPacket
    {
        public readonly MPKeyPress MPKeyPress;
        
        public ServerUpdateDataPacket(MPKeyPress mPKeyPress)
        {
            MPKeyPress = mPKeyPress;
        }
    }
}
