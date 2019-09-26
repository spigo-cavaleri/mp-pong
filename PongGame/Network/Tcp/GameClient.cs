using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using PongGame.Network.JSONGeneric;
using PongGame.Network.Tcp.Data;

namespace PongGame.Network.Tcp
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
        private ConcurrentQueue<TcpDataPacket> packetsToSend;
        private ConcurrentQueue<TcpDataPacket> packetsToReceive;
        #endregion

        #region CONSTRUCTERS
        /// <summary>
        /// Constructs a GameClient
        /// </summary>
        /// <param name="client">The underlying Tcp Client used for connecting to the host</param>
        public GameClient(TcpClient client)
        {
            // initialize the queues use form data wrapping
            packetsToReceive = new ConcurrentQueue<TcpDataPacket>();
            packetsToSend = new ConcurrentQueue<TcpDataPacket>();

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
        /// Sends object to the server, serializing it using json
        /// </summary>
        /// <typeparam name="T">The object type to send</typeparam>
        /// <param name="dataPacket">The object to serialize and send to the server</param>
        public void SetDataToSend<T>(T dataPacket)
        {
            T dataToSendPacket;

            lock (dataPacket)
            {
                dataToSendPacket = dataPacket;
            }

            Thread sendThread = new Thread(() =>
            {
                if (JSONSerializer.SerializeData(dataPacket, out string data))
                {
                    packetsToSend.Enqueue(new TcpDataPacket(this, data));
                }
            });
        }

        /// <summary>
        /// Gets the next data packet in the data packets to receive queue
        /// </summary>
        /// <typeparam name="T">The type of object to recieve</typeparam>
        /// <returns>The object of data</returns>
        public T GetNextDataToReceive<T>()
        {
            // !packetsToReceive.IsEmpty &&
            if (packetsToReceive.TryDequeue(out TcpDataPacket tcpDataPacket))
            {
                // Tries to serialize the data packet as T type, if it failes then enqueues the datapacket back into the list
                if (JSONSerializer.DeSerializeData(tcpDataPacket.Data, out T dataPacket))
                {
                    return dataPacket;
                }
                else
                {
                    // if deserializer failes puts the tcp data packet back into the queue, this should be reworked in a later update
                    packetsToReceive.Enqueue(tcpDataPacket);
                }
            }

            return default(T);
        }

        /// <summary>
        /// Gets the latest data that the client has received from the server
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <returns>The object of data</returns>
        public T GetLatestDataToReceive<T>()
        {
            T[] tmpArray = GetAllDataToReceive<T>();

            if (tmpArray.Length > 0)
            {
                int lastElementNumber = tmpArray.Length - 1;
                return tmpArray[lastElementNumber];
            }

            return default(T);
        }

        /// <summary>
        /// Receives the object sendt from the server
        /// </summary>
        /// <typeparam name="T">The type of object expected to receice</typeparam>
        /// <returns>An array of object matching the type param, empty if no data is sendt</returns>
        public T[] GetAllDataToReceive<T>()
        {
            // Temporary bag for storing all the packets to get
            ConcurrentBag<T> packets = new ConcurrentBag<T>();

            int currentPacketIndex = packetsToReceive.Count;

            for (int i = currentPacketIndex; i >= 0; i--)
            {
                packets.Add(GetNextDataToReceive<T>());
            }

            return packets.ToArray();
        }

        /// <summary>
        /// Disposes the instance of the underlying tcp connection, and set IsRunning to false;
        /// </summary>
        public void CloseConnection()
        {
            IsRunning = false;

            TcpClient.Close();
        }

        /// <summary>
        /// To string override.
        /// </summary>
        /// <returns>Returns the gameclients name and if it is running</returns>
        public override string ToString()
        {
            return string.Format("GameClient: {0}, is running {1}", Name, IsRunning);
        }
        #endregion

        #region INTERNAL FUNCTIONS
        /// <summary>
        /// This is a server function and shouldn't be called anywhere else !!
        /// Returns an array of data packets that is received by this client
        /// </summary>
        /// <returns>The data packets received from the server, an empty array if no data is to receive</returns>
        internal TcpDataPacket[] GetDataToReceiveServer()
        {
            // Temporary bag for storing all the packets to get
            ConcurrentBag<TcpDataPacket> packets = new ConcurrentBag<TcpDataPacket>();

            while (packetsToReceive != null && packetsToReceive.Count > 0)
            {
                if (packetsToReceive.TryDequeue(out TcpDataPacket packet))
                {
                    packets.Add(packet);
                }
            }

            return packets.ToArray();
        }

        /// <summary>
        /// This is a server function and shouldn't be called anywhere else !!
        /// Controls if the client in the data packet matches this client, if it doesn't then the data isn't sendt
        /// </summary>
        /// <param name="data">The data to send, if data is empty or null nothing is sendt</param>
        internal void SetDataToSendServer(TcpDataPacket data)
        {
            if (packetsToSend != null && data.Client == this && !string.IsNullOrEmpty(data.Data))
            {
                packetsToSend.Enqueue(data);
            }
        }
        #endregion

        #region PRIVATE FUNCTIONS
        /// <summary>
        /// The client loop that sends and receives data to and from a tcp server
        /// </summary>
        private void ClientLoop()
        {
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
                            packetsToReceive.Enqueue(new TcpDataPacket(this, data));
                        }
                    }

                    // Sends all packets to the server
                    while (packetsToSend.Count > 0)
                    {
                        if (packetsToSend.TryDequeue(out TcpDataPacket nextPacketToSend))
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
                CloseConnection();
            }
        }
        #endregion
    }
}