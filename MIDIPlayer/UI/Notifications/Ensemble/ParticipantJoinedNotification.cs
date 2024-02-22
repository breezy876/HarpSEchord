using Common.Models.Ensemble;
using Hscm.UI.Notifications;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications
{
    public class ParticipantJoinedNotification : MessageBase
    {
        public FFXIVCharacter Participant { get; set; }
    }
}
