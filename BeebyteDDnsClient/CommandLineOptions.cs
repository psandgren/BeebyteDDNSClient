using CommandLine;

namespace BeebyteDDnsClient
{
    public class CommandLineOptions
    {
        [Value(index: 0, MetaName = "<hostname>", Required = true, HelpText = "Which hostname to update")]
        public string Hostname { get; set; }

        [Option(shortName: 'k', longName: "key", Required = true, HelpText = "Beebyte API key")]
        public string ApiKey { get; set; }

        [Option(shortName: 'i', longName: "ip", Required = false, HelpText = "The new ip for the hostname")]
        public string Ip { get; set; }

        [Option(shortName: 'v', longName: "verbose", Required = false, HelpText = "Set output to verbose messages")]
        public bool Verbose { get; set; }
    }
}
