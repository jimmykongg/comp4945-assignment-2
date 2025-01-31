using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace UnityGame.Server;

public class WebSocketServer
{
    private static CancellationTokenSource cts = new CancellationTokenSource();
    
    private static async Task AcceptClients(HttpListener listener, CancellationToken token)
    {
        Console.WriteLine("WebSocket Server started, waiting for connections...");

        while (!token.IsCancellationRequested)
        {
            HttpListenerContext context = await listener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                Console.WriteLine("New WebSocket connection request received");
                HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
                WebSocket webSocket = wsContext.WebSocket;
                
                _ = Task.Run(() => HandleConnectionAsync(webSocket, token));
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
                Console.WriteLine("HTTP connection rejected, WebSocket only");
            }
        }
    }

    private static async Task HandleConnectionAsync(WebSocket webSocket, CancellationToken token)
    {
        byte[] buffer = new byte[1024];

        try
        {
            while (webSocket.State == WebSocketState.Open && !token.IsCancellationRequested)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), token);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message: {receivedMessage}");

                // Broadcast the received message to all clients
                byte[] responseBuffer = Encoding.UTF8.GetBytes(receivedMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true,
                    token);

                if (result.CloseStatus.HasValue)
                    break;
            }
        }
        finally
        {
            if (webSocket.State != WebSocketState.Closed)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
            Console.WriteLine("WebSocket connection closed");
        }
    }

    static void Main(string[] args)
    {
        string serverUrl = "http://localhost:8080/";
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(serverUrl);

        listener.Start();
        Console.WriteLine($"WebSocket server running at {serverUrl}");
        
        Task serverTask = Task.Run(async () => await AcceptClients(listener, cts.Token));
        Console.WriteLine("Press ENTER to stop the server...");
        Console.ReadLine();
        
        cts.Cancel();
        listener.Stop();
        Console.WriteLine("Server stopped");
    }
}