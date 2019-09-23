using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using PongGame.Network.JSONGeneric;
using PongGame.Network.Tcp.Data;

namespace PongGame.Network.Tcp
{
    /// <summary>
    /// A game server that can send and receive from a game client the server uses Tcp Protocol. 
    /// This is a sealed class and can't be inherited from.
    /// </summary>
    public sealed class GameServer
    {
        #region EVENTS
        /// <summary>
        /// The game server exception event, used for getting the thrown socket exceptions in the game server
        /// </summary>
        public event GameServerExceptionHandle GameServerException;
        public delegate void GameServerExceptionHandle(string exceptionMessage);
        #endregion

        #region SINGLETON
        public static GameServer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameServer();
                }

                return instance;
            }
        }

        private static GameServer instance;
        #endregion

        #region PROPERTIES
        /// <summary>
        /// Returns true if the server is running, false otherwise
        /// </summary>
        public bool IsRunning
        {
            get;
            private set;
        } = true;

        /// <summary>
        /// The underlying Tcp listener
        /// </summary>
        public TcpListener TcpListener
        {
            get;
        }

        /// <summary>
        /// The local endpoint of this server
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get;
        }
        #endregion

        #region PRIVATE FIELDS
        private ConcurrentQueue<TcpDataPacket> packetsToSend;
        private ConcurrentQueue<TcpDataPacket> packetsToReceive;

        private ConcurrentBag<GameClient> gameClients;
        #endregion

        #region CONSTRUCTERS
        /// <summary>
        /// Constructs a game server
        /// </summary>
        private GameServer()
        {
            try
            {
                ushort port = WanUtils.GetRandomPortNumber();
                IPAddress ip = WanUtils.WanIPV4Address;

                TcpListener = new TcpListener(ip, port);

                LocalEndPoint = new IPEndPoint(ip, port);

                gameClients = new ConcurrentBag<GameClient>();

                packetsToSend = new ConcurrentQueue<TcpDataPacket>();
                packetsToReceive = new ConcurrentQueue<TcpDataPacket>();

                Thread serverThread = new Thread(ServerLoop)
                {
                    Name = "ServerThread",
                    IsBackground = false
                };

                Console.WriteLine("Server constructed");

                Start(serverThread);

            }
            catch (SocketException e)
            {
                if (GameServerException != null)
                {
                    GameServerException(string.Format("Game server Socket exception: {0}", e));
                }

                Stop();
            }
        }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// The current list of clients in the server
        /// </summary>
        /// <returns>if there're game clients on the server returns them, otherwise returns null</returns>
        public GameClient[] GetGameClients()
        {
            if (gameClients != null)
            {
                return gameClients.ToArray();
            }

            return null;
        }

        /// <summary>
        /// Gets the names of all clients on the server list
        /// </summary>
        /// <returns>returns the names of the clients, otherwise returns null</returns>
        public string[] GetClientNames()
        {
            ConcurrentBag<string> clientNames = new ConcurrentBag<string>();

            foreach (GameClient gameClient in gameClients)
            {
                clientNames.Add(gameClient.Name);
            }

            return clientNames.ToArray();
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            IsRunning = false;

            TcpListener.Stop();
        }

        /// <summary>
        /// Broadcast message to all cconnected clients
        /// </summary>
        /// <param name="data">The data to broadcast</param>
        public void BroadCast<T>(T dataPacket)
        {
            if (gameClients != null)
            {
                if (JSONSerializer.SerializeData(dataPacket, out string data))
                {
                    // enqueues a Tcp packet for each client in the current client list
                    foreach (GameClient client in gameClients)
                    {
                        packetsToSend.Enqueue(new TcpDataPacket(client, data));
                    }
                }
            }
        }

        /// <summary>
        /// Sends data to a specific client via the tcp data packet
        /// </summary>
        /// <param name="data"></param>
        public void SendToClient<T>(GameClient client, T dataPacket)
        {
            if (client != null && JSONSerializer.SerializeData(dataPacket, out string data))
            {
                TcpDataPacket packet = new TcpDataPacket(client, data);
                // Enqueue the data to send to client in the servers tcp packet queue
                packetsToSend.Enqueue(packet);
            }
        }

        /// <summary>
        /// Gets the data that is received by the server
        /// </summary>
        /// <typeparam name="T">The type of object expected to receive</typeparam>
        /// <returns>Returns an array of Tcp packets if any, otherwise returns empty</returns>
        public T[] GetDataToReceive<T>()
        {
            // Temporary bag for storing all the packets to get
            ConcurrentBag<T> packets = new ConcurrentBag<T>();

            if (packetsToReceive != null)
            {
                while (packetsToReceive.Count > 0)
                {
                    if (packetsToReceive.TryDequeue(out TcpDataPacket packet))
                    {
                        if (JSONSerializer.DeSerializeData(packet.Data, out T dataPacket))
                        {
                            packets.Add(dataPacket);
                        }
                    }
                }
            }

            return packets.ToArray();
        }
        #endregion

        #region PRIVATE FUNCTIONS
        /// <summary>
        /// Starts the server and the server loop thead, this is run in the constructer
        /// </summary>
        /// <param name="serverThread">The thead that handles the server loop</param>
        private void Start(Thread serverThread)
        {
            TcpListener.Start();
            serverThread.Start();
        }

        /// <summary>
        /// The server loop that handles all clients and packets to receive and send
        /// </summary>
        private void ServerLoop()
        {
            try
            {
                while (IsRunning)
                {
                    Pending();

                    if (gameClients.Count > 0)
                    {
                        RemoveDisconnectedClients();

                        RemoveDisconnectedClientUnsendtDataPackets();

                        Receive();

                        Send();
                    }
                }
            }
            catch (SocketException e)
            {
                if (GameServerException != null)
                {
                    GameServerException(string.Format("Server Socket exception: {0}", e));
                }
            }
        }

        /// <summary>
        /// Looks for pending clients that is requesting the server to accept them
        /// </summary>
        private void Pending()
        {
            if (TcpListener.Pending())
            {
                // the tcp client that is pending to connect to the server
                TcpClient pendingClient = TcpListener.AcceptTcpClient();

                // if the pending client isn't null add it to te client list
                if (pendingClient != null)
                {
                    GameClient newClient = new GameClient(pendingClient);

                    gameClients.Add(newClient);
                }
            }
        }

        /// <summary>
        /// Receives packets from all clients on the server
        /// </summary>
        private void Receive()
        {
            foreach (GameClient client in gameClients)
            {
                // temp array of the packets to receive from the current client
                TcpDataPacket[] packets = client.GetDataToReceiveServer();
                if (packets != null)
                {
                    for (int i = 0; i < packets.Length; i++)
                    {
                        // Enqueues the packets from the client into the server received packets queue
                        packetsToReceive.Enqueue(packets[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Sends packets to all clients there is a packet for in the servers packet send queue
        /// </summary>
        private void Send()
        {
            for (int i = 0; i < packetsToSend.Count; i++)
            {
                foreach (GameClient client in gameClients)
                {
                    if (packetsToSend.TryPeek(out TcpDataPacket peekPacket))
                    {
                        // the next packet to send is on the client list
                        if (client == peekPacket.Client)
                        {
                            TcpDataPacket packetForFoundClient;
                            // tries to dequeue the packet from the packet to send queue
                            while (!packetsToSend.TryDequeue(out packetForFoundClient))
                            { }

                            // sends the data to the client in the client list 
                            client.SetDataToSendServer(packetForFoundClient);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes disconnected clients from the client list
        /// </summary>
        private void RemoveDisconnectedClients()
        {
            foreach (GameClient client in gameClients)
            {
                // Sees if the client is disconnected
                if (!client.TcpClient.Connected)
                {
                    // closes the connection on the client in server list
                    client.CloseConnection();

                    // tries to remove the client from the list until it succeeds
                    while (!gameClients.TryTake(out GameClient disconnectedClient))
                    { }
                }
            }
        }

        /// <summary>
        /// Removes packets to send if the client has disconnected
        /// </summary>
        private void RemoveDisconnectedClientUnsendtDataPackets()
        {
            // Peeks the next packet in the send packet queue
            if (packetsToSend.TryPeek(out TcpDataPacket peekPacket))
            {
                foreach (GameClient client in gameClients)
                {
                    // if the packet has a receiver then it returns to the main server loop
                    if (client.Name == peekPacket.Client.Name)
                    {
                        return;
                    }

                    // tries to remove the packet if the client has disconnected, it keeps trying until it suceeds
                    while (!packetsToSend.TryDequeue(out TcpDataPacket packetForDisconnectedCLient))
                    { }
                }
            }
        }
    }
    #endregion
}