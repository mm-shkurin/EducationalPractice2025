using System;

namespace task17
{
    public class HardStop : ICommand
    {
        private readonly ServerThread _serverThread;

        public HardStop(ServerThread serverThread)
        {
            _serverThread = serverThread ?? throw new ArgumentNullException(nameof(serverThread));
        }

        public void Execute()
        {
            _serverThread.HardStop();
        }
    }
} 