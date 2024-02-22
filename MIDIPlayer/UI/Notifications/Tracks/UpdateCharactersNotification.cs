
using Common.Music;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications.Tracks
{
    public class UpdateCharactersNotification : SequenceNotification
    {
        public string CharacterName { get; set; }
        public bool IsSelected { get; set; }
    }
}
