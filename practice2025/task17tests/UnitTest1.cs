using System;
using System.Threading;
using Xunit;
using task17;

namespace task17tests
{
    public class ServerThreadTests
    {
        [Fact]
        public void TestServerThreadCreation()
        {
            using var server = new ServerThread();
            Assert.True(server.IsRunning);
        }

        [Fact]
        public void TestAddCommand()
        {
            using var server = new ServerThread();
            var command = new TestCommand("test");
            server.AddCommand(command);
            
            Thread.Sleep(100);
            Assert.True(server.IsRunning);
        }

        [Fact]
        public void TestHardStop()
        {
            using var server = new ServerThread();
            var hardStopCommand = new HardStop(server);
            server.AddCommand(hardStopCommand);
            
            server.WaitForCompletion();
            Assert.False(server.IsRunning);
        }

        [Fact]
        public void TestSoftStop()
        {
            using var server = new ServerThread();
            var testCommand = new TestCommand("test");
            var softStopCommand = new SoftStop(server);
            
            server.AddCommand(testCommand);
            server.AddCommand(softStopCommand);
            
            server.WaitForCompletion();
            Assert.False(server.IsRunning);
        }

        [Fact]
        public void TestHardStopFromWrongThread()
        {
            using var server = new ServerThread();
            Assert.Throws<InvalidOperationException>(() => server.HardStop());
        }

        [Fact]
        public void TestSoftStopFromWrongThread()
        {
            using var server = new ServerThread();
            Assert.Throws<InvalidOperationException>(() => server.SoftStop());
        }

        [Fact]
        public void TestHardStopImmediateStop()
        {
            using var server = new ServerThread();
            var hardStopCommand = new HardStop(server);
            
            server.AddCommand(hardStopCommand);
            
            var startTime = DateTime.Now;
            server.WaitForCompletion();
            var endTime = DateTime.Now;
            
            Assert.False(server.IsRunning);
            Assert.True((endTime - startTime).TotalMilliseconds < 100);
        }

        [Fact]
        public void TestSoftStopWaitsForQueue()
        {
            using var server = new ServerThread();
            var command1 = new TestCommand("1", 100);
            var command2 = new TestCommand("2", 100);
            var softStopCommand = new SoftStop(server);
            
            server.AddCommand(command1);
            server.AddCommand(command2);
            server.AddCommand(softStopCommand);
            
            var startTime = DateTime.Now;
            server.WaitForCompletion();
            var endTime = DateTime.Now;
            
            Assert.False(server.IsRunning);
            Assert.True((endTime - startTime).TotalMilliseconds >= 200);
        }

        [Fact]
        public void TestExceptionHandling()
        {
            using var server = new ServerThread();
            var exceptionCommand = new TestCommand("exception", 0, true);
            ICommand capturedCommand = null;
            Exception capturedException = null;
            
            server.ExceptionHandler += (command, exception) =>
            {
                capturedCommand = command;
                capturedException = exception;
            };
            
            server.AddCommand(exceptionCommand);
            Thread.Sleep(100);
            
            Assert.NotNull(capturedCommand);
            Assert.NotNull(capturedException);
            Assert.IsType<InvalidOperationException>(capturedException);
        }

        [Fact]
        public void TestAddCommandToStoppedServer()
        {
            using var server = new ServerThread();
            var hardStopCommand = new HardStop(server);
            server.AddCommand(hardStopCommand);
            server.WaitForCompletion();
            
            var testCommand = new TestCommand("test");
            Assert.Throws<InvalidOperationException>(() => server.AddCommand(testCommand));
        }

        [Fact]
        public void TestMultipleCommandsExecution()
        {
            using var server = new ServerThread();
            var commands = new TestCommand[5];
            
            for (int i = 0; i < 5; i++)
            {
                commands[i] = new TestCommand($"cmd{i}");
            }
            
            foreach (var command in commands)
            {
                server.AddCommand(command);
            }
            
            var softStopCommand = new SoftStop(server);
            server.AddCommand(softStopCommand);
            
            server.WaitForCompletion();
            Assert.False(server.IsRunning);
        }
    }
}