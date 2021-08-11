using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommandLine;

namespace BeebyteDDnsClient
{
    internal class Program
    {
        private static readonly string _ddnsHost = "https://dynupdate.beebyte.se";
        private static readonly string _ddnsIpHost = "https://dynupdate-ip.beebyte.se";


        internal static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(async (CommandLineOptions opts) =>
                {
                    try
                    {
                        // We have the parsed arguments, so let's just pass them down
                        return await DynDnsUpdate(opts).ConfigureAwait(false);
                    }
                    catch
                    {
                        Console.WriteLine("Error!");
                        return -3; // Unhandled error
                    }
                },
                errs => Task.FromResult(-1)); // Invalid arguments
        }

        internal static async Task<int> DynDnsUpdate(CommandLineOptions options)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    if (options.Verbose)
                    {
                        Console.WriteLine();
                        Console.WriteLine("=== SSL Certificate Error ===");
                        Console.WriteLine($"== Sender: \n\r{sender}\n\r");
                        Console.WriteLine($"== Certificate info: \n\r{cert}");
                        Console.WriteLine($"== Chain: \n\r{chain}\n\r");
                        Console.WriteLine($"== SslPolicyErrors: \n\r{sslPolicyErrors}\n\r");
                    }
                    return true;
                }
            };

            HttpClient httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes(
                            $"einstein:{options.ApiKey}")));

            HttpRequestMessage message;
            if (string.IsNullOrWhiteSpace(options.Ip))
            {
                message = new HttpRequestMessage(HttpMethod.Get, $"{_ddnsIpHost}/nic/update?hostname={options.Hostname}");
            }
            else
            {
                message = new HttpRequestMessage(HttpMethod.Get, $"{_ddnsHost}/nic/update?hostname={options.Hostname}&myip={options.Ip}");
            }

            HttpResponseMessage response = await httpClient.SendAsync(message).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            if (options.Verbose)
            {
                string data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Console.WriteLine("=== Beebyte response ===");
                Console.WriteLine(data);
                Console.WriteLine();
            }

            Console.WriteLine("Record updated!");
            return 0;
        }
    }
}
