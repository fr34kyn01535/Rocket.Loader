using Rocket.API;
using System.Xml.Serialization;

namespace Rocket.Unturned.Settings
{
    public class ImplementationSettings : IRocketImplementationConfigurationSection
    {
        [XmlArrayItem(ElementName = "WhitelistedGroup")]
        public string[] WhitelistedGroups = new string[0];

        [XmlArrayItem(ElementName = "ReservedSlotsGroup")]
        public string[] ReservedSlotsGroups = new string[0];

        [XmlElement(ElementName = "ReservedSlots")]
        public int ReservedSlots = 0;

        [XmlElement(ElementName = "AutomaticShutdownClearLevel")]
        public bool AutoShutdownClearLevel = false;

        [XmlElement(ElementName = "AutomaticShutdownClearPlayers")]
        public bool AutomaticShutdownClearPlayers = false;
    }
}
