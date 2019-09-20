using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using PongGame.Data;

namespace PongGame.Tcp
{
    /// <summary>
    /// A game client used for multiplayer gaming, used with the game server.
    /// This is a sealed class and can't be inherited from.
    /// </summary>
    public sealed class GameClient
    {
        #region EVENTS
        /// <summary>
        /// The game client exception event, used for getting the thrown socket exceptions in the game client
        /// </summary>
        public event GameClientExceptionHandle GameClientException;
        public delegate void GameClientExceptionHandle(string exceptionMessage);
        #endregion

        #region PROPERTIES
        /// <summary>
        /// ´Returns true if the client is running, false otherwise
        /// </summary>
        public bool IsRunning
        {
            get;
            private set;
        } = true;

        /// <summary>
        /// The name of the client, this is made out of the ip address and port number of the client
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// The underlying Tcp Client
        /// </summary>
        public TcpClient TcpClient
        {
            get;
        }
        #endregion

        #region PRIVATE FIELDS
        private ConcurrentQueue<TcpDataPacket> PacketsToSend;
        private ConcurrentQueue<TcpDataPacket> PacketsToReceive;
        #endregion

        #region CONSTRUCTERS
        /// <summary>
        /// Constructs a GameClient
        /// </summary>
        /// <param name="client">The underlying Tcp Client used for connecting to the host</param>
        public GameClient(TcpClient client)
        {
            try
            {
                TcpClient = client;

                IPEndPoint endpoint = TcpClient.Client.LocalEndPoint as IPEndPoint;
                Name = string.Format("{0}:{1}", endpoint.Address, endpoint.Port);

                Thread clientThread = new Thread(ClientLoop)
                {
                    IsBackground = true,
                    Name = "ClientThread"
                };

                clientThread.Start();
            }
            catch (SocketException e)
            {
                if (GameClientException != null)
                {
                    GameClientException(string.Format("Socket exception: {0}", e));
                }

                CloseConnection();
            }
        }

        /// <summary>
        /// Construct a GameClient
        /// </summary>
        /// <param name="hostIpAddress">The host address</param>
        /// <param name="hostPort">The host port</param>
        public GameClient(string hostIpAddress, ushort hostPort) :
                     this(new TcpClient(hostIpAddress, hostPort))
        { }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// The current game client
        /// </summary>
        /// <returns>This game client </returns>
        public GameClient GetGameClient()
        {
            return this;
        }

        /// <summary>
        /// Sends a data to the server
        /// </summary>
        /// <param name="data">The data to send, if data is empty or null nothing is sendt</param>
        public void SetDataToSend(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                SetDataToSend(new TcpDataPacket(this, data));
            }
        }

        /// <summary>
        /// Sends data to the server, 
        /// controls if the client in the data packet matches this client, if it doesn't then the data isn't sendt
        /// </summary>
        /// <param name="data">The data to send, if data is empty or null nothing is sendt</param>
        public void SetDataToSend(TcpDataPacket data)
        {
            if (PacketsToSend != null && data.Client == this && !string.IsNullOrEmpty(data.Data))
            {
                PacketsToSend.Enqueue(data);
            }
        }

        /// <summary>
        /// Returns an array of data packets that is received by this client
        /// </summary>
        /// <returns>The data packets received from the server, null if the array is empty</returns>
        public TcpDataPacket[] GetDataToReceive()
        {
            if (PacketsToReceive != null && PacketsToReceive.Count > 0)
            {
                // Temporary bag for storing all the packets to get
                ConcurrentBag<TcpDataPacket> packets = new ConcurrentBag<TcpDataPacket>();

                while (PacketsToReceive.Count > 0)
                {
                    if (PacketsToReceive.TryDequeue(out TcpDataPacket packet))
                    {
                        packets.Add(packet);
                    }
                }

                return packets.ToArray();
            }

            return new ConcurrentBag<TcpDataPacket>().ToArray();
        }

        /// <summary>
        /// Disposes the instance of the underlying tcp connection, and set IsRunning to false;
        /// </summary>
        public void CloseConnection()
        {
            IsRunning = false;

            TcpClient.Close();
        }

        public override string ToString()
        {
            return string.Format("GameClient: {0}, is running {1}", Name, IsRunning);
        }
        #endregion

        #region PRIVATE FUNCTIONS
        private void ClientLoop()
        {
            // initialize the queues use form data wrapping
            PacketsToReceive = new ConcurrentQueue<TcpDataPacket>();
            PacketsToSend = new ConcurrentQueue<TcpDataPacket>();

            StreamReader reader = new StreamReader(TcpClient.GetStream(), Encoding.ASCII);
            StreamWriter writer = new StreamWriter(TcpClient.GetStream(), Encoding.ASCII);

            try
            {
                while (TcpClient.Connected || IsRunning)
                {
                    // Receive all packets from the server   
                    if (TcpClient.GetStream().DataAvailable)
                    {
                        string data = null;
                        data = reader.ReadLine();

                        if (!string.IsNullOrEmpty(data))
                        {
                            PacketsToReceive.Enqueue(new TcpDataPacket(this, data));
                        }
                    }

                    // Sends all packets to the server
                    while (PacketsToSend.Count > 0)
                    {

                        if (PacketsToSend.TryDequeue(out TcpDataPacket nextPacketToSend))
                        {
                            writer.WriteLine(nextPacketToSend.Data);
                            writer.Flush();
                        }
                    }
                }
            }
            catch (IOException e)
            {
                if (GameClientException != null)
                {
                    GameClientException(string.Format("Client IO exception: {0}", e));
                }
            }
            catch (ArgumentException e)
            {
                if (GameClientException != null)
                {
                    GameClientException(string.Format("Client Argument exception: {0}", e));
                }
            }
            catch (SocketException e)
            {
                if (GameClientException != null)
                {
                    GameClientException(string.Format("Client Socket exception: {0}", e));
                }
            }
            finally
            {
                reader.Dispose();
                writer.Dispose();
                TcpClient.Close();
                CloseConnection();
            }
        }
        #endregion
    }
}
