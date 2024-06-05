using Mono.Options;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net;
using TranslationsBuilder;
using TranslationsBuilder.Services;

namespace TranslationsBuilderConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // http://www.ndesk.org/doc/ndesk-options/NDesk.Options/OptionSet.html
            string inputFormat = "";
            string inputFile = "";
            string pbixFile = "";
            bool showHelp = false;

            var p = new OptionSet() {
                { "h|help",  "show this message and exit", v => showHelp = v != null },
                { "i|inputFormat=",  "input file format (required)", v => inputFormat = v },
                { "o|inputFile=",  "input file path (required)", v => inputFile = v },
                { "v|pbixFile=",    "target PBIX file path (required)", v => pbixFile = v }
            };

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            } catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try using --help' for more information.");
                Environment.Exit(0);
            }

            // We either run Power BI Desktop ourselves, or have the pipeline run it

            var process = Process.GetProcessesByName("msmdsrv")[0];
            var parent = process.GetParent();

            var tcpTable = ManagedIpHelper.GetExtendedTcpTable();
            var tcpRow = tcpTable.SingleOrDefault((r) => r.ProcessId == process.Id && r.State == TcpState.Listen && IPAddress.IsLoopback(r.LocalEndPoint.Address));

            TranslationsManager.Connect("localhost:" + tcpRow?.LocalEndPoint.Port.ToString());

            return;

            if (showHelp ||
                inputFormat.Equals("") ||
                inputFile.Equals("") ||
                pbixFile.Equals(""))
            {
                ShowHelp(p);
                Environment.Exit(0);
            }
        }
        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: PayloadGenerator [OPTIONS]+");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
            Console.WriteLine("-------------------");
        }
    }
}