using System;

namespace OpenCGSS.Tools.BeatmapConverter.Cgss {
    public sealed class Note : ICloneable {

        public int ID { get; set; }
        public double HitTiming { get; set; }
        public NoteType Type { get; set; }
        public NotePosition StartPosition { get; set; }
        public NotePosition FinishPosition { get; set; }
        public NoteStatus FlickType { get; set; }
        public bool IsSync { get; set; }
        public int GroupID { get; set; }

        public Note NextHoldNote { get; set; }
        public Note NextFlickNote { get; set; }
        public Note PrevHoldNote { get; set; }
        public Note PrevFlickNote { get; set; }
        public Note SyncPairNote { get; set; }
        public Note NextSlideNote { get; set; }
        public Note PrevSlideNote { get; set; }

        public bool HasNextHold => NextHoldNote != null;
        public bool HasNextFlick => NextFlickNote != null;
        public bool HasNextSlide => NextSlideNote != null;
        public bool HasPrevHold => PrevHoldNote != null;
        public bool HasPrevFlick => PrevFlickNote != null;
        public bool HasPrevSlide => PrevSlideNote != null;
        public bool IsFlick => Type == NoteType.TapOrFlick && (FlickType == NoteStatus.FlickLeft || FlickType == NoteStatus.FlickRight);
        public bool IsTap => Type == NoteType.TapOrFlick && FlickType == NoteStatus.Tap;
        public bool IsHold => Type == NoteType.Hold;
        public bool IsHoldStart => Type == NoteType.Hold && HasNextHold;
        public bool IsHoldEnd => Type == NoteType.TapOrFlick && HasPrevHold;
        public bool IsSlide => Type == NoteType.Slide;
        public bool IsSlideStart => Type == NoteType.Slide && HasNextSlide;
        public bool IsSlideMidway => Type == NoteType.Slide && HasNextSlide && HasPrevSlide;
        public bool IsSlideEnd => Type == NoteType.Slide && HasPrevSlide;
        public bool IsGamingNote => Type == NoteType.TapOrFlick || Type == NoteType.Hold || Type == NoteType.Slide;

        public Note Clone() {
            var note = new Note {
                ID = ID,
                HitTiming = HitTiming,
                Type = Type,
                StartPosition = StartPosition,
                FinishPosition = FinishPosition,
                FlickType = FlickType,
                IsSync = IsSync,
                GroupID = GroupID,
                NextHoldNote = NextHoldNote,
                PrevHoldNote = PrevHoldNote,
                NextFlickNote = NextFlickNote,
                PrevFlickNote = PrevFlickNote,
                SyncPairNote = SyncPairNote,
                NextSlideNote = NextSlideNote,
                PrevSlideNote = PrevSlideNote
            };
            return note;
        }

        object ICloneable.Clone() {
            return Clone();
        }

        public override string ToString() {
            return $"ID: {ID}, Timing: {HitTiming}, Type: {Type}";
        }
    }
}
