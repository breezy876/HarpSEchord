﻿using Common.Music;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications
{
    public class SequenceNotification : MessageBase
    {
        public MidiSequence Sequence { get; set; }
    }
}