using System.ComponentModel;

namespace OpenCGSS.Tools.BeatmapConverter.Cgss {
    public enum NoteStatus {

        [Description("Tap")]
        Tap = 0,
        [Description("Flick left")]
        FlickLeft = 1,
        [Description("Flick right")]
        FlickRight = 2

    }
}
