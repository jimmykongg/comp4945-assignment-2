using System;
using System.Threading.Tasks;

namespace TagGame.Plugins
{

    public interface INetwork
    {
        event Action<string> OnMessageReceived;

        void SendMessage(string message);

        Task StartListening();

        void StopListening();
    }
}