using System.Runtime.Serialization;

namespace PongGame.Network.Tcp.Data
{
    /// <summary>
    /// A data packet to update client game
    /// </summary>
    [DataContract]
    public struct ClientUpdateDataPacket
    {
        #region PUBLIC FIELDS
        /// <summary>
        /// Server player position on the Y-axis
        /// </summary>
        [DataMember]
        public readonly int SPPositionY;

        /// <summary>
        /// Client player position on the Y-axis
        /// </summary>
        [DataMember]
        public readonly int CPPositionY;

        /// <summary>
        /// Ball position on the X-axis
        /// </summary>
        [DataMember]
        public readonly int BallPositionX;

        /// <summary>
        /// Ball position on the Y-axis
        /// </summary>
        [DataMember]
        public readonly int BallPositionY;
        #endregion

        #region CONSTUCTERS
        /// <summary>
        /// Constructs a update data packet for the client
        /// </summary>
        /// <param name="sPPositionY">Server player position on the Y-axis</param>
        /// <param name="cPPositionY">Client player position on the Y-axis</param>
        /// <param name="bPositionX">Ball position on the X-axis</param>
        /// <param name="bPositionY">Ball position on the Y-axis</param>
        public ClientUpdateDataPacket(int sPPositionY, int cPPositionY, int bPositionX, int bPositionY)
        {
            SPPositionY = sPPositionY;
            CPPositionY = cPPositionY;
            BallPositionX = bPositionX;
            BallPositionY = bPositionY;
        }
        #endregion
    }
}
