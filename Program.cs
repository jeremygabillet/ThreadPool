using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ThreadPoolQuest
{
    class Program
    {
        private CountdownEvent _countdownEvent;
        private int _threadsCount;
        private ConcurrentStack<int> _concurrentStack;
        static void Main(string[] args)
        {
            Program program = new Program(5);
            program.Run(1000000);
        }
        public Program(int threadsCount)
        {
            _threadsCount = threadsCount;
            _countdownEvent = new CountdownEvent(threadsCount);
            _concurrentStack = new ConcurrentStack<int>();
        }
        public void Run(int numIntegers)
        {
            Random random = new Random();
            for (int i = 0; i < numIntegers/_threadsCount; i++)
            {
                for (int j = 0; j < _threadsCount; j++)
                {
                    ThreadPool.QueueUserWorkItem(x =>
                    {
                        _concurrentStack.Push(random.Next(0, 1000000));
                        _countdownEvent.Signal();
                    });
                }
                _countdownEvent.Wait();
                _countdownEvent.Reset(_threadsCount);
            }
            System.Console.WriteLine($"Concurrent stack reached {_concurrentStack.Count} integers.");
        }
    }
}
