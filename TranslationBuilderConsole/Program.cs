using Mono.Options;

namespace TranslationBuyilderConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // http://www.ndesk.org/doc/ndesk-options/NDesk.Options/OptionSet.html
            bool show_help = false;
            string inputfile = "";
            string outputdir = "";
            Version version = new Version();

            var p = new OptionSet() {
                { "h|help",  "show this message and exit", v => show_help = v != null },
                { "i|inputfile=",  "input xml file (required)", v => inputfile = v },
                { "o|outputdir=",  "output directory (if not specified, then input file name will be used)", v => outputdir = v },
                { "v|version=",    "target Business Central version", v => version = new Version(v) }
            };
        }
    }
}