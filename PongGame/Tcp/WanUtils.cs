using System;
using System.Net;
using System.Net.Sockets;

namespace PongGame.Tcp
{
    /// <summary>
    /// Usefull Wan utilities
    /// </summary>
    public static class WanUtils
    {
        #region ENUM
        /// <summary>
        /// Ip address to try and ping
        /// </summary>
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
        /// The Wan ip exception event, used for getting the thrown socket exceptions if it fails
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
        private static readonly Random randomPort = new Random();
        #endregion

        #region PUBLIC FUNCTIONS
        public static ushort GetRandomPortNumber()
        {
            return (ushort)randomPort.Next(49153, ushort.MaxValue - 1);
        }
        #endregion


        #region PRIVATE FUCNTIONS
        private static IPAddress GetLocalIpAddress()
        {
            IPAddress wanAddress = null;

            int tryCounter = 0, tryCounterMax = 6;

            while (wanAddress == null)
            {
                try
                {
                    ushort portNumber = GetRandomPortNumber();

                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
                    {
                        socket.Connect(ConnectionIpAddresses(tryCounter), portNumber);

                        IPEndPoint endpoint = socket.LocalEndPoint as IPEndPoint;

                        wanAddress = endpoint.Address;
                    }

                    return wanAddress;
                }
                catch (SocketException e)
                {
                    if (WanIpException != null)
                    {
                        WanIpException(string.Format("Wan IPV4 Exception, tries {1} out of {2}: {0}", e, tryCounter, tryCounterMax));
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
