namespace UnityGame.Plugins;

public interface INetwork
{
    event Action<string> OnMessageReceived;

    void SendMessage(string message);

    Task StartListening();
    
    void StopListening();
}