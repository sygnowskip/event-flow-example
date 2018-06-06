using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Payments.StressTests
{
    public class PaymentProcessTest
    {
        private string ExternalId { get; }
        private Uri ApiBaseUrl { get; }
        private HttpClient HttpClient { get; } = new HttpClient();
        private int PingCounts { get; }
        private readonly IList<long> _responseTimes = new List<long>();
        private bool Verbose { get; }

        public PaymentProcessTest(string apiBaseUrl, bool verbose)
        {
            Verbose = verbose;
            PingCounts = new Random(DateTime.Now.Millisecond).Next(10, 50);
            ExternalId = Path.GetRandomFileName().Replace(".", "");
            ApiBaseUrl = new Uri(apiBaseUrl);
        }

        public async Task Start()
        {
            await BeginProcess();
            for (var i = 0; i < PingCounts; i++)
            {
                await Ping();
            }

            await Cancel();
            PrintSummary();
        }

        public void PrintSummary()
        {
            Console.WriteLine($"--- SUMMARY FOR {ExternalId} ---");
            Console.WriteLine($"Total calls: {_responseTimes.Count}, average response time: {string.Format("{0:N2}", GetAverageResponseTime()) } miliseconds");
            Console.WriteLine("--------------------------------");
        }

        public double GetAverageResponseTime()
        {
            return (double)_responseTimes.Sum() / _responseTimes.Count;
        }

        private async Task BeginProcess()
        {
            var beginUrl = new Uri(ApiBaseUrl, "/api/payment/begin");
            var request = new
            {
                Country = "Poland",
                Currency = "PLN",
                System = "ExampleAPI",
                ExternalId = ExternalId,
                ExternalCallbackUrl = "http://example.api/callback",
                Amount = 149.99
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            await Execute("BeginProcess", async () =>
                await HttpClient.PostAsync(beginUrl, content));
        }

        private async Task Ping()
        {
            var pingUrl = new Uri(ApiBaseUrl, $"/api/payment/ping?externalId={ExternalId}");
            await Execute("Ping", async () =>
                await HttpClient.GetAsync(pingUrl));
        }

        private async Task Cancel()
        {
            var cancelUrl = new Uri(ApiBaseUrl, $"/api/testprovider1/cancel?externalId={ExternalId}");
            await Execute("Cancel", async () =>
                await HttpClient.GetAsync(cancelUrl));
        }

        private async Task Execute(string httpCallName, Func<Task<HttpResponseMessage>> httpCall)
        {
            var timer = Stopwatch.StartNew();
            await httpCall();
            timer.Stop();
            if (Verbose)
            {
                Console.WriteLine($"{httpCallName} for {ExternalId} in {timer.ElapsedMilliseconds} miliseconds");
            }
            _responseTimes.Add(timer.ElapsedMilliseconds);
        }
    }
}