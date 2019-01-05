using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace AspNetCoreReproFor6415.Client
{
    /// <summary>
    /// Console application that acts as a client for reproducing issue with HTTP POST
    /// requests when using IIS in-process hosting if the request body is not buffered.
    /// </summary>
    internal static class Program
    {
        internal static async Task Main(string[] args)
        {
            // URL matches IIS Express URL in AspNetCoreReproFor6415
            // Switch between unbuffered or buffered request body by specifying --buffered as an argument
            var hostUrl = "http://localhost:50692";
            var useBuffering = string.Equals(args.FirstOrDefault(), "--buffered", StringComparison.OrdinalIgnoreCase);

            Console.WriteLine($"Host URL           : {hostUrl}");
            Console.WriteLine($"Buffer request body: {useBuffering}");

            // Create request body content
            var content = new MyModel() { Y = "foo" };

            content.X["a"] = 1.1;
            content.X["b"] = 2.2;
            content.X["c"] = 3.3;

            // Create Refit client
            var requests = 0;
            var client = RestService.For<IClient>(hostUrl);

            // Create a delegate to the buffered or unbuffered POST request method
            var request = useBuffering ?
                new Func<Task>(() => client.PostBufferedAsync("a", "b", "c", content)) :
                new Func<Task>(() => client.PostUnbufferedAsync("a", "b", "c", content));

            var stopwatch = Stopwatch.StartNew();

            using (var cts = new CancellationTokenSource())
            {
                Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

                // Issue HTTP POST requests to the same resource continually
                // until either Ctrl+C is pressed or an exception occurs.
                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        await request();

                        if (requests++ % 100 == 0)
                        {
                            Console.Write('.');
                        }
                    }
                    catch (ApiException ex)
                    {
                        Console.Error.WriteLine();
                        Console.Error.WriteLine($"Response body: {ex.Content}");
                        Console.Error.WriteLine(ex.ToString());

                        cts.Cancel();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine();
                        Console.Error.WriteLine(ex.ToString());
                        cts.Cancel();
                    }
                }
            }

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine($"{requests} requests made successfully in {stopwatch.Elapsed}.");
            Console.WriteLine();

            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
