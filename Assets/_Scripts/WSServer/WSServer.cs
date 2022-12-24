using Fleck;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class WSServer : MonoBehaviour
{
    public static WSServer wSServer { get; private set; }

    List<IWebSocketConnection> connections = new List<IWebSocketConnection>();
    public TMP_Text ipText;

    private static ClientWebSocket ws;

    static Queue<string> log = new Queue<string>();

    public static async Task ConnectAsync(Uri uri)
    {
        string connection = await FindServersAsync();
        if(connection != null)
        {
            log.Enqueue("found " + connection);
            uri = new Uri(connection);
        }
        ws = new ClientWebSocket();
        await ws.ConnectAsync(uri, CancellationToken.None);
    }

    public static async Task SendMessageAsync(string message)
    {
        if (ws == null || ws.State != WebSocketState.Open)
        {
            throw new InvalidOperationException("WebSocket is not open");
        }

        var buffer = System.Text.Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(buffer);
        await ws.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        Debug.Log("sent: " + message);
    }

    // Start is called before the first frame update
    void Start()
    { 
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList.LastOrDefault
            (a => a.AddressFamily == AddressFamily.InterNetwork);
        Debug.Log(ipAddress.ToString());
        ipText.text = ipAddress.ToString();
        if (wSServer != null)
        {
            return;
        }
        wSServer = this;
        DontDestroyOnLoad(this);

        ConnectAsync( new Uri("ws://127.0.0.1:9515"));
        /*ws = new WebSocketSharp.WebSocket("ws://127.0.0.1:7638");
        ws.Connect();
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Connected to server");
            // Connection to the WebSocket server was successful
        };

        ws.OnMessage += (sender, e) =>
        {
            // A message was received from the WebSocket server
            string message = e.Data;
        };

        ws.OnError += (sender, e) =>
        {
            // An error occurred while communicating with the WebSocket server
            Debug.LogError(e.Message);
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.LogWarning("Disconected");
            // The WebSocket connection was closed
        };*/


        //https://github.com/statianzo/Fleck
        /*var server = new WebSocketServer("ws://127.0.0.1:7638");
        server.Start(socket =>
        {
            socket.OnOpen = () => {Debug.Log("Open!"); connections.Add(socket); };
            socket.OnClose = () => {Debug.Log("Close!"); connections.Remove(socket); };
            socket.OnMessage = message => {Debug.Log(socket.ConnectionInfo.ClientIpAddress + message); };
        });*/
    }

    private void LateUpdate()
    {
        if(log.Count > 0)
        {
            string s = log.Dequeue();
            Debug.Log(s);
            ipText.text = s;
        }
    }
    public static void SendToAllClients(string message)
    {
        if (wSServer?.connections != null)
        {
            foreach (IWebSocketConnection connection in wSServer.connections)
            {
                connection.Send(message);
            }
        }
        SendMessageAsync(message);
    }


    public static async Task<string> FindServersAsync()
    {
        // Determine the range of IP addresses on the local network
        string localIP = GetLocalIPAddress();
        string[] parts = localIP.Split('.');
        string baseIP = parts[0] + "." + parts[1] + "." + parts[2] + ".";
        for (int i = 0; i < 256; i++)
        {
            string testIP = baseIP + i;
            if (testIP == localIP)
            {
                // Skip the local IP address
                //continue;
            }
            testIP = "ws://" + testIP + ":9515";

            // Try to connect to a WebSocket server at this IP address
            try
            {
                using (var ws = new ClientWebSocket())
                {
                    try
                    {
                        log.Enqueue("trying" + testIP.ToString());
                        var cts = new CancellationTokenSource();
                        var timer = new Timer(_ => cts.Cancel(), null, 600, Timeout.Infinite);
                        await ws.ConnectAsync(new Uri(testIP), cts.Token);
                        log.Enqueue("Connected to WebSocket server at " + new Uri(testIP));
                        return testIP;
                    }
                    catch (System.Net.WebSockets.WebSocketException ex)
                    {
                        log.Enqueue("Error connecting to WebSocket server: " + ex.Message);
                    }catch (Exception ex)
                    {
                        log.Enqueue("Error connecting to WebSocket server: " + ex.Message);
                    }
                }
            }
            catch (SocketException)
            {
                // No WebSocket server at this IP address
                return null;
            }
        }

        return null;
    }

    private static string GetLocalIPAddress()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList.LastOrDefault
            (a => a.AddressFamily == AddressFamily.InterNetwork);
        return ipAddress.ToString();
    }
}
