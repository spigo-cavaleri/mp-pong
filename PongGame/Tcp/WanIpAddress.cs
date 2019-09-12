using System;
using System.Net;
using System.Net.Sockets;

namespace PongGame.Tcp
{
    /// <summary>
    /// Finds the wan IPV4 on the current mashine that is connected to the internet
    /// </summary>
    public static class WanIpAddress
    {
        #region ENUM
        private enum DNSIPAddress
        {
            GooglePrimaryDns = 0,
            GoogleSecondaryDns = 1,
            CloudFlarePrimaryDns = 2,
            CloudFlareSecondaryDns = 3,
            OpenDnsPrimaryDns = 4,
            OpenDnsSecondaryDns = 5
        }
        #endregion

        #region EVENTS
        /// <summary>
        /// Returns an exception from the wan IP address class
        /// </summary>
        public static event WanIpExceptionHandle WanIpException;
        public delegate void WanIpExceptionHandle(string message);
        #endregion

        #region PROPERTIES
        /// <summary>
        /// Returns the Wan IPV4 address of this mashine
        /// </summary>
        public static IPAddress WanIPV4Address
        {
            get
            {
                if (wanIp == null)
                {
                    wanIp = GetLocalIpAddress();
                }

                return wanIp;
            }
        }
        #endregion

        #region PRIVATE FIELDS
        private static IPAddress wanIp;
        #endregion

        #region PRIVATE FUCNTIONS
        private static IPAddress GetLocalIpAddress()
        {
            IPAddress wanAddress = null;
            Random randomPort = new Random();
            int tryCounter = 0, tryCounterMax = 6;

            while (wanAddress == null)
            {
                try
                {
                    ushort portNumber = (ushort)randomPort.Next(60000, ushort.MaxValue - 1);

                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
                    {
                        socket.Connect(ConnectionIpAddresses(tryCounter), portNumber);

                        IPEndPoint endpoint = socket.LocalEndPoint as IPEndPoint;

                        wanAddress = endpoint.Address;
                    }

                    return wanAddress;
                }
                catch (Exception e)
                {
                    if(WanIpException != null)
                    {
                        WanIpException(string.Format("Wan IPV4 Exception: {0}, on {1} out of {2} tries", e, tryCounter, tryCounterMax));
                    }
                }

                if (tryCounter == tryCounterMax)
                {
                    break;
                }

                tryCounter++;
            }

            if (WanIpException != null)
            {
                WanIpException("No connection to the internet, the Wan Ip is set to 127.0.0.1");
            }

            return IPAddress.Loopback;
        }

        private static string ConnectionIpAddresses(int value)
        {
            DNSIPAddress address;

            address = (DNSIPAddress)value;

            switch (address)
            {
                case DNSIPAddress.GooglePrimaryDns:
                    return "8.8.8.8";

                case DNSIPAddress.GoogleSecondaryDns:
                    return "8.8.4.4";

                case DNSIPAddress.CloudFlarePrimaryDns:
                    return "1.1.1.1";

                case DNSIPAddress.CloudFlareSecondaryDns:
                    return "1.0.0.1";

                case DNSIPAddress.OpenDnsPrimaryDns:
                    return "208.67.222.222";

                case DNSIPAddress.OpenDnsSecondaryDns:
                    return "208.67.220.220";
            }

            return "37.235.1.174";
        }
        #endregion
    }
}
