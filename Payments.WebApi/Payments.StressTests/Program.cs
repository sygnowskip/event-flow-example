using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.StressTests
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {
            var apiUrls = new[] { "http://localhost:60563", "http://localhost:60564" };
            var numberOfThreads = 8;
            Console.WriteLine($"STARTING STRESS TESTS WITH {numberOfThreads} THREADS");
            var tasks = new List<Task>();
            var responseTimes = new ConcurrentBag<double>();
            var externalId = Path.GetRandomFileName().Replace(".", "");
            var paymentProcessCreator = new PaymentProcessTest(apiUrls[0], true, externalId);
            await paymentProcessCreator.BeginProcess();
            for (var i = 0; i < numberOfThreads; i++)
            {
                var urlIndex = i % 2;
                tasks.Add(Task.Run(async () =>
                {
                    var test = new PaymentProcessTest(apiUrls[urlIndex], true, externalId);
                    await test.Start();
                    foreach (var responseTime in test.ResponseTimes)
                    {
                        responseTimes.Add(responseTime);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            await paymentProcessCreator.Cancel();
            Console.WriteLine($"--- SUMMARY FOR ALL CALLS ---");
            Console.WriteLine($"Average response time: {string.Format("{0:N2}", responseTimes.Sum() / responseTimes.Count) } miliseconds");
            Console.WriteLine("--------------------------------");
            Console.ReadKey();
        }
    }

    
}
