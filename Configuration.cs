using Rocket.API;
using Rocket.Core.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Rocket.Unturned.Items;
using SDG.Unturned;

namespace StrikesPlugin
{
    public sealed class Strike
    {
        [XmlAttribute("SequenceNumber")]
        public int SequenceNumber;
        [XmlAttribute("Action")]
        public string Action;
        [XmlAttribute("BanOrKickReason")]
        public string Reason;
        [XmlAttribute("BanTime")]
        public uint BanTime;
        [XmlAttribute("ClearWarnings")]
        public bool ClearWarnings;

        public Strike(int sequencenumber, string action, string reason, uint bantime, bool clearwarnings)
        {
            SequenceNumber = sequencenumber;
            Action = action;
            Reason = reason;
            BanTime = bantime;
            ClearWarnings = clearwarnings;
        }
        public Strike()
        {
            SequenceNumber = 0;
            Action = "none";
            Reason = "";
            BanTime = 0;
            ClearWarnings = false;
        }
    }

    public class Configuration : IRocketPluginConfiguration
    {
        public bool AnnounceKickAndBanGlobally;
        public bool AnnounceStrikesGlobally;
        public List<Strike> StrikeSequence;

        public void LoadDefaults()
        {
            AnnounceKickAndBanGlobally = true;
            AnnounceStrikesGlobally = true;

            StrikeSequence = new List<Strike>
            {
                new Strike(1, "", "", 0, false),
                new Strike(2, "", "", 0, false),
                new Strike(3, "", "", 0, false),
                new Strike(4, "", "", 0, false),
                new Strike(5, "BAN", "You have reached your 5th strike", 18000, true)
            };
        }
    }
}
