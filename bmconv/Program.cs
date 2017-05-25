using System;
using System.IO;
using System.Text;
using CommandLine.Text;
using OpenCGSS.Tools.BeatmapConverter.Cgss;
using OpenCGSS.Tools.BeatmapConverter.Deleste;
using OpenCGSS.Tools.BeatmapConverter.Extensions;

namespace OpenCGSS.Tools.BeatmapConverter {
    internal static class Program {

        private static int Main(string[] args) {
            var options = new Options();
            var isValid = CommandLine.Parser.Default.ParseArguments(args, options);
            var helpText = HelpText.AutoBuild(options);
            helpText.AddPreOptionsLine($"Usage:{Environment.NewLine}  bmconv <input file> [options]");

            if (!isValid) {
                helpText.OutputError(null);
                return 1;
            }

            if (string.IsNullOrEmpty(options.InputFileName)) {
                helpText.OutputError("You have to specify an input file.");
                return 2;
            }

            FileInfo inputFileInfo, outputFileInfo;

            try {
                inputFileInfo = new FileInfo(options.InputFileName);
            } catch (Exception ex) {
                helpText.OutputError(ex.Message);
                return -1;
            }

            if (!inputFileInfo.Exists) {
                helpText.OutputError($"Input file '{options.InputFileName}' does not exist.");
                return 3;
            }

            switch (options.ConversionType) {
                case ConversionTypes.ToTxt:
                    break;
                default:
                    helpText.OutputError($"Unrecognized conversion target: '{options.ConversionType}'.");
                    return 4;
            }

            var isCsv = inputFileInfo.Extension.ToLowerInvariant() == ".csv";

            Difficulty selectedDifficulty;
            if (isCsv) {
                selectedDifficulty = Difficulty.Master;
            } else {
                selectedDifficulty = (Difficulty)options.Difficulty;
            }

            if (!isCsv) {
                // Check other parameters
                if (selectedDifficulty < Difficulty.Debut || selectedDifficulty > Difficulty.MasterPlus) {
                    helpText.OutputError($"Unrecognized difficulty value: {(int)selectedDifficulty}.");
                    return 5;
                }

                var bdbResult = Score.IsScoreFile(inputFileInfo.FullName);
                if (!bdbResult.IsValid) {
                    helpText.OutputError($"File '{inputFileInfo.FullName}' is not a valid BDB file.");
                    return 6;
                }

                if (!Score.ContainsDifficulty(bdbResult.SupportedNames, selectedDifficulty)) {
                    helpText.OutputError($"File '{inputFileInfo.FullName}' does not contain a difficulty {(int)selectedDifficulty} beatmap.");
                    return 7;
                }
            }

            Score score = null;
            try {
                score = isCsv ? Score.FromCsvFile(inputFileInfo.FullName) : Score.FromBdbFile(inputFileInfo.FullName, selectedDifficulty);
            } catch (Exception ex) {
                helpText.OutputError(ex.Message);
                return -2;
            }

            // Now lets begin the conversion!
            try {
                if (string.IsNullOrEmpty(options.OutputFileName)) {
                    string extension;
                    switch (options.ConversionType) {
                        case ConversionTypes.ToTxt:
                            extension = ".txt";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("This should not happen.");
                    }
                    options.OutputFileName = Path.Combine(inputFileInfo.DirectoryName, inputFileInfo.GetSafeFileName() + extension);
                }
                outputFileInfo = new FileInfo(options.OutputFileName);
            } catch (Exception ex) {
                helpText.OutputError(ex.Message);
                return -3;
            }

            switch (options.ConversionType) {
                case ConversionTypes.ToTxt: {
                        try {
                            using (var fileStream = outputFileInfo.Open(FileMode.Create, FileAccess.Write)) {
                                using (var writer = new StreamWriter(fileStream, Encoding.UTF8)) {
                                    DelesteHelper.WriteDelesteBeatmap(score, selectedDifficulty, writer);
                                }
                            }
                        } catch (Exception ex) {
                            helpText.OutputError(ex.Message);
                            return -4;
                        }
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException("This should not happen.");
            }

            Console.WriteLine("Conversion completed. Contents are written to '{0}'.", outputFileInfo.FullName);

            return 0;
        }

        private static void OutputError(this HelpText helpText, string message) {
            if (!string.IsNullOrEmpty(message)) {
                Console.Error.WriteLine(message);
                Console.Error.WriteLine();
            }
            Console.Error.WriteLine(helpText.ToString());
        }

    }
}
