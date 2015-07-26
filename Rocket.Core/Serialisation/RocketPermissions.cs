using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rocket.Core.Serialisation
{
    [Serializable]
    public class RocketPermissions
    {
        public RocketPermissions()
        {
        }

        [XmlElement("DefaultGroupId")]
        public string DefaultGroupId = "default";

        [XmlElement("Groups")]
        [XmlArrayItem("Group")]
        public RocketPermissionsGroup[] Groups = new RocketPermissionsGroup[] {
            new RocketPermissionsGroup("default","Guest",new List<string>(), null , new List<string>() { "p", "reward","balance","pay","rocket","color.white" }),
            new RocketPermissionsGroup("vip","VIP", new List<string>(),new List<string>() { "76561198016438091" }, new List<string>() { "color.FF9900" })
        };
    }
}
