namespace UnityGame.Plugins;

using System.Net.WebSockets;
using System.Text;

public class WebsocketNetwork : INetwork
{
    private ClientWebSocket webSocket;
    private const string ServerUrl = "ws://localhost:8080/";
    private CancellationTokenSource cts = new CancellationTokenSource();
    
    public event Action<string> OnMessageReceived;

    public WebsocketNetwork()
    {
        webSocket = new ClientWebSocket();
    }
    
    public async void SendMessage(string message)
    {
        if (webSocket.State == WebSocketState.Open)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cts.Token);
        }
    }

    public async Task StartListening()
    {
        try
        {
            await webSocket.ConnectAsync(new Uri(ServerUrl), cts.Token);
            byte[] buffer = new byte[1024];

            while (webSocket.State == WebSocketState.Open && !cts.Token.IsCancellationRequested)
            {
                WebSocketReceiveResult result =
                    await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);

                OnMessageReceived?.Invoke(receivedMessage);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error when trying to receive message: " + e);
        }
    }

    public void StopListening()
    {
        cts.Cancel();
        webSocket.Abort();
    }
}