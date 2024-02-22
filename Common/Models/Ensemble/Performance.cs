using Common;
using Common.Messaging;
using Common.Messaging.Player;
using Common.Models.Settings;
using Common.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Ensemble
{

    public class Ensemble
    {

        public Ensemble()
        {
            Characters = new List<FFXIVCharacter>();

            Settings = new Common.Models.Ensemble.Settings();
        }

        public List<FFXIVCharacter> Characters { get; set; }

        public Common.Models.Ensemble.Settings Settings { get; set; }

        public int Total => Characters.Count;

        public List<FFXIVCharacter> GetSelected()
        {
            if (Characters.IsNullOrEmpty())
                return null;

            return Characters.Where(p => p.IsSelected).ToList();
        }

        public void Reset()
        {
            //Settings.ParticipantSettings.Clear();
            Characters.Clear();
        }

        public void RemoveParticipant(FFXIVCharacter participant)
        {
            //if (!Settings.ParticipantSettings.ContainsKey(participant.CharacterName))
            //    return;

            //Settings.ParticipantSettings.Remove(participant.CharacterName);
        }

        //TODO deep clone playlist for participant management
        public void AddParticipant(FFXIVCharacter participant)
        {
            //if (Settings.ParticipantSettings.ContainsKey(participant.CharacterName))
            //    return;

            //var songSettings = BinarySerializer.Clone(Common.Settings.PlaylistSettings);

            //Settings.ParticipantSettings.Add(participant.CharacterName, songSettings);
        }
    }
}
