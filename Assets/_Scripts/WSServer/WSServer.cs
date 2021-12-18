using Fleck;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class WSServer : MonoBehaviour
{
    public static WSServer wSServer { get; private set; }

    List<IWebSocketConnection> connections = new List<IWebSocketConnection>();
    // Start is called before the first frame update
    void Start()
    {
        if(wSServer != null)
        {
            return;
        }
        DontDestroyOnLoad(this);
        wSServer = this;

        //https://github.com/statianzo/Fleck
        var server = new WebSocketServer("ws://127.0.0.1:8080");
        server.Start(socket =>
        {
            socket.OnOpen = () => {Debug.Log("Open!"); connections.Add(socket); };
            socket.OnClose = () => {Debug.Log("Close!"); connections.Remove(socket); };
            socket.OnMessage = message => {Debug.Log(socket.ConnectionInfo.ClientIpAddress + message); };
        });
    }

    public static void SendToAllClients(string message)
    {
        if (wSServer.connections != null)
        {
            foreach (IWebSocketConnection connection in wSServer.connections)
            {
                connection.Send(message);
            }
        }
    }

}
