using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.StressTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiUrl = "http://localhost:60563";
            var numberOfThreads = 50;
            Console.WriteLine($"STARTING STRESS TESTS WITH {numberOfThreads} THREADS");
            var tasks = new List<Task>();
            var responseTimes = new ConcurrentBag<double>();
            for (var i = 0; i < numberOfThreads; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var test = new PaymentProcessTest(apiUrl, true);
                    await test.Start();
                    foreach (var responseTime in test.ResponseTimes)
                    {
                        responseTimes.Add(responseTime);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"--- SUMMARY FOR ALL CALLS ---");
            Console.WriteLine($"Average response time: {string.Format("{0:N2}", responseTimes.Sum() / responseTimes.Count) } miliseconds");
            Console.WriteLine("--------------------------------");
            Console.ReadKey();
        }
    }

    
}
