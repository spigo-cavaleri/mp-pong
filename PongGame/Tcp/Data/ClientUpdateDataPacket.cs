using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongGame.Tcp.Data
{
    public struct ClientUpdateDataPacket
    {
        public readonly int SPPositionY;
        public readonly int CPPositionY;
        public readonly int BallPositionX;
        public readonly int BallPositionY;

        public ClientUpdateDataPacket(int sPPositionY, int cPPositionY, int bPositionX, int bPositionY)
        {
            SPPositionY = sPPositionY;
            CPPositionY = cPPositionY;
            BallPositionX = bPositionX;
            BallPositionY = bPositionY;
        }
    }
}
