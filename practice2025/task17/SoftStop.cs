using System;

namespace task17
{
    public class SoftStop : ICommand
    {
        private readonly ServerThread _serverThread;

        public SoftStop(ServerThread serverThread)
        {
            _serverThread = serverThread ?? throw new ArgumentNullException(nameof(serverThread));
        }

        public void Execute()
        {
            _serverThread.SoftStop();
        }
    }
} 