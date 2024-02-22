using Common.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Midi
{

    public class PlayerInfo
    {
        public PlayerInfo(
            PlayerState playerState,
            SequenceTimeSpan timeElapsed,
            SequenceTimeSpan timeLeft,
            int total,
            int percentage,
            long position)
        {
            this.PlayerState = playerState;
            this.TimeElapsed = timeElapsed;
            this.TimeLeft = timeLeft;
            this.Position = position;
        }

        public PlayerState PlayerState { get; private set; }
        public SequenceTimeSpan TimeElapsed { get; private set; }
        public SequenceTimeSpan TimeLeft { get; private set; }

        public long Position { get; private set; }
    }
}
