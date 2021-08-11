# BeebyteDDNSClient
A commandline DynDNS client for the provider [Beebyte.se](https://www.beebyte.se). Built with .NET5.0.

## Requirements
- .NET5 runtime
- A beebyte-account and an api-key

## Usage

### Without ip

    BeebyteDDnsClient.exe <hostname> -k <apikey>

This will set the ip for the hostname to your current public ip from where the call is made.

### With ip

    BeebyteDDnsClient.exe <hostname> -i <ip> -k <apikey>

This will set the ip for the hostname to your input.