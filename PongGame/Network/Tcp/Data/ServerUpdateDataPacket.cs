using PongGame.MPPongGame;

namespace PongGame.Network.Tcp.Data
{
    /// <summary>
    /// A data packet for the server to update the client players position
    /// </summary>
    public struct ServerUpdateDataPacket
    {
        #region PUBLIC FIELDS
        /// <summary>
        /// The movement intent of the client player
        /// </summary>
        public readonly MPKeyPress MPKeyPress;
        #endregion

        #region CONSTUCTERS
        /// <summary>
        /// Constructs a data packet to update the intent of the client players position
        /// </summary>
        /// <param name="mPKeyPress">The movement intent of the client player</param>
        public ServerUpdateDataPacket(MPKeyPress mPKeyPress)
        {
            MPKeyPress = mPKeyPress;
        }
        #endregion
    }
}
