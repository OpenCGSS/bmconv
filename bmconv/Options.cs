using CommandLine;

namespace OpenCGSS.Tools.BeatmapConverter {
    public sealed class Options {

        [ValueOption(0)]
        public string InputFileName { get; set; }

        [Option('t', "to", DefaultValue = ConversionTypes.ToTxt, HelpText = "Conversion type. Available: txt.")]
        public string ConversionType { get; set; }

        [Option("difficulty", DefaultValue = 0, HelpText = "The specified difficulty when opening a beatmap bundle (BDB).")]
        public int Difficulty { get; set; }

        [Option('o', "out", HelpText = "Output file location.")]
        public string OutputFileName { get; set; }

    }
}
