using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rocket.Core.Serialisation
{
    [Serializable]
    public class RocketPermissionsGroup
    {
        public RocketPermissionsGroup()
        {
        }

        public RocketPermissionsGroup(string name, string displayName, List<string> parentGroups, List<string> members, List<string> commands)
        {
            Id = name;
            DisplayName = displayName;
            Members = members;
            Commands = commands;
            ParentGroups = parentGroups;
        }

        [XmlElement("Id")]
        public string Id;

        [XmlElement("DisplayName")]
        public string DisplayName;

        [XmlElement("Members")]
        [XmlArrayItem(ElementName = "Member")]
        public List<string> Members;

        [XmlElement("Commands")]
        [XmlArrayItem(ElementName = "Command")]
        public List<string> Commands;

        [XmlElement("ParentGroups")]
        [XmlArrayItem(ElementName = "ParentGroup")]
        public List<string> ParentGroups;
    }
}
