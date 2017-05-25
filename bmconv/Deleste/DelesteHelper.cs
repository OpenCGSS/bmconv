using System;
using System.IO;
using OpenCGSS.Tools.BeatmapConverter.Cgss;

namespace OpenCGSS.Tools.BeatmapConverter.Deleste {
    internal static class DelesteHelper {

        public static void WriteDelesteBeatmap(Score score, Difficulty difficulty, TextWriter writer) {
            WriteBeatmapHeader(score, difficulty, writer);
            WriteEntries(score, writer);
        }

        private static void WriteBeatmapHeader(Score score, Difficulty difficulty, TextWriter writer) {
            writer.WriteLine("#utf8");
            writer.WriteLine("#Title (Title Here)");
            writer.WriteLine("#Lyricist (Lyricist)");
            writer.WriteLine("#Composer (Composer)");
            writer.WriteLine("#Background background.jpg");
            writer.WriteLine("#Song song.ogg");
            writer.WriteLine("#Lyrics lyrics.lyr");
            // Using the 240/0 preset as discussed at 2016-10-09. May be updated when a new version of Deleste Viewer is released.
            //writer.WriteLine("#BPM {0:F2}", score.Project.Settings.GlobalBpm);
            //writer.WriteLine("#Offset {0}", (int)Math.Round(score.Project.Settings.StartTimeOffset * 1000));
            writer.WriteLine("#BPM 240");
            // Don't know why, but the 40 ms lag shows up from nowhere.
            writer.WriteLine("#Offset -40");
            string s;
            switch (difficulty) {
                case Difficulty.Debut:
                case Difficulty.Regular:
                case Difficulty.Pro:
                case Difficulty.Master:
                    s = difficulty.ToString();
                    break;
                case Difficulty.MasterPlus:
                    s = "Master+";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            writer.WriteLine("#Difficulty {0}", s);
            switch (difficulty) {
                case Difficulty.Debut:
                    s = "7";
                    break;
                case Difficulty.Regular:
                    s = "13";
                    break;
                case Difficulty.Pro:
                    s = "17";
                    break;
                case Difficulty.Master:
                    s = "23";
                    break;
                case Difficulty.MasterPlus:
                    s = "30";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            writer.WriteLine("#Level {0}", s);
            writer.WriteLine("#BGMVolume 100");
            writer.WriteLine("#SEVolume 100");
            writer.WriteLine("#Attribute All");
            // Use the undocumented "Convert" format for CSV-converted beatmaps.
            writer.WriteLine("#Format Convert");
            writer.WriteLine();
        }

        private static void WriteEntries(Score score, TextWriter writer) {
            // group_id,measure_index:deleste_note_type:start_position:finish_position
            // * measure_index can be floating point numbers: 000.000000
            foreach (var note in score.Notes) {
                if (!note.IsGamingNote) {
                    continue;
                }
                var groupID = note.GroupID;
                var hitTiming = note.HitTiming;
                var noteType = TranslateNoteType(note);
                var startPosition = note.StartPosition;
                var finishPosition = note.FinishPosition;
                writer.WriteLine("#{0},{1:000.000000}:{2}:{3}:{4}", groupID, hitTiming, (int)noteType, (int)startPosition, (int)finishPosition);
            }
        }

        private static DelesteNoteType TranslateNoteType(Note note) {
            switch (note.Type) {
                case NoteType.TapOrFlick:
                    switch (note.FlickType) {
                        case NoteStatus.Tap:
                            return DelesteNoteType.Tap;
                        case NoteStatus.FlickLeft:
                            return DelesteNoteType.FlickLeft;
                        case NoteStatus.FlickRight:
                            return DelesteNoteType.FlickRight;
                        default:
                            // Should have thrown an exception.
                            return DelesteNoteType.Tap;
                    }
                case NoteType.Hold:
                    return DelesteNoteType.Hold;
                case NoteType.Slide:
                    if (note.HasNextFlick) {
                        var nextFlick = note.NextFlickNote;
                        if (nextFlick.FinishPosition > note.FinishPosition) {
                            return DelesteNoteType.FlickRight;
                        } else if (nextFlick.FinishPosition < note.FinishPosition) {
                            return DelesteNoteType.FlickLeft;
                        } else {
                            throw new ArgumentOutOfRangeException("Unsupported flick type for slide notes.");
                        }
                    } else {
                        return DelesteNoteType.Slide;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
