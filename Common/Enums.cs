using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum PlayerState { NotStarted, Stopped, Playing, Paused, Finished }

    public enum ReduceType { Fixed, Random }

    public enum PlaybackType {  Original, Preview }

    public enum TackAutofillType { Settings, Midi }

    public enum RepeatType
    {
        LoopNone, LoopOne, LoopAll, Shuffle
    }

    public enum MouseWheelFunction
    {
        Seek,
        Tempo
    }

    public enum HotKeyType
    {
        Play,
        Next,
        Prev,
        Stop,
        Preview,
        OctaveUp,
        OctaveDown,
        KeyUp,
        KeyDown,
        RevertSong,
        RevertTempo,
        ResetTempo,
        MouseWheelShift
    }

    public enum EnsembleMode { Network, Local }
}
