
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications.Tracks
{
    public class RemoveTracksNotification : MessageBase
    {
        public IEnumerable<int> TrackIndexes { get; set; }
    }
}
