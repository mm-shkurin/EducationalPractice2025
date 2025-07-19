using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task17
{
    public class ServerThread : IDisposable
    {
        private readonly ConcurrentQueue<ICommand> _commandQueue;
        private readonly Thread _thread;
        private volatile bool _isRunning;
        private volatile bool _softStopRequested;

        public ServerThread()
        {
            _commandQueue = new ConcurrentQueue<ICommand>();
            _isRunning = true;
            _softStopRequested = false;
            _thread = new Thread(ProcessCommands);
            _thread.Start();
        }

        public void AddCommand(ICommand command)
        {
            if (!_isRunning)
                throw new InvalidOperationException("Server thread is not running");
            
            _commandQueue.Enqueue(command);
        }

        public void HardStop()
        {
            if (Thread.CurrentThread != _thread)
                throw new InvalidOperationException("HardStop can only be called from the server thread");
            
            _isRunning = false;
        }

        public void SoftStop()
        {
            if (Thread.CurrentThread != _thread)
                throw new InvalidOperationException("SoftStop can only be called from the server thread");
            
            _softStopRequested = true;
        }

        private void ProcessCommands()
        {
            while (_isRunning)
            {
                if (_commandQueue.TryDequeue(out ICommand command))
                {
                    try
                    {
                        command.Execute();
                        if (!_isRunning)
                            break;
                    }
                    catch (Exception ex)
                    {
                        HandleException(command, ex);
                    }
                }
                else
                {
                    if (_softStopRequested)
                    {
                        _isRunning = false;
                        break;
                    }
                    
                    Thread.Sleep(1);
                }
            }
        }

        private void HandleException(ICommand command, Exception exception)
        {
            try
            {
                ExceptionHandler?.Invoke(command, exception);
            }
            catch
            {
            }
        }

        public void WaitForCompletion()
        {
            _thread.Join();
        }

        public bool IsRunning => _isRunning;

        public event Action<ICommand, Exception> ExceptionHandler;

        public void Dispose()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _thread.Join(1000);
            }
        }
    }
} 