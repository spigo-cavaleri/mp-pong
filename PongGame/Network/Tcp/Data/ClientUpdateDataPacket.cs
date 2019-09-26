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

        [DataMember]
        public readonly int SPoints;

        [DataMember]
        public readonly int SHealth;

        [DataMember]
        public readonly int CPoints;

        [DataMember]
        public readonly int CHealth;
        #endregion

        #region CONSTUCTERS
        /// <summary>
        /// Constructs a update data packet for the client
        /// </summary>
        /// <param name="sPPositionY">Server player position on the Y-axis</param>
        /// <param name="cPPositionY">Client player position on the Y-axis</param>
        /// <param name="bPositionX">Ball position on the X-axis</param>
        /// <param name="bPositionY">Ball position on the Y-axis</param>
        public ClientUpdateDataPacket(int sPPositionY, int cPPositionY, int bPositionX, int bPositionY, int SPoints, int SHealth, int CPoints, int CHealth)
        {
            SPPositionY = sPPositionY;
            CPPositionY = cPPositionY;
            BallPositionX = bPositionX;
            BallPositionY = bPositionY;

            this.SPoints = SPoints;
            this.SHealth = SHealth;
            this.CPoints = CPoints;
            this.CHealth = CHealth;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;

            ClientUpdateDataPacket other = (ClientUpdateDataPacket)obj;

            return other.SPPositionY == this.SPPositionY &&
                   other.CPPositionY == this.CPPositionY &&
                   other.BallPositionX == this.BallPositionX &&
                   other.BallPositionX == this.BallPositionY &&
                   other.SPoints == this.SPoints &&
                   other.SHealth == this.SHealth &&
                   other.CPoints == this.CPoints &&
                   other.CHealth == this.CHealth;
        }
        #endregion
    }
}
