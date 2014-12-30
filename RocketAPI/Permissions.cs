﻿using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Rocket.RocketAPI
{
    public static class Permissions
    {
        private static string permissionsFile = "Servers/" + Bootstrap.InstanceName + "/Rocket/Permissions.config";

        private static List<Group> defaultGroups = new List<Group>() { new Group("default", new List<string>() { "76561198016438091" }, new List<string>() { "plugins", "vote", "reward" }) };
        private static List<Group> Groups = null;

        public static void Load()
        {
            if (File.Exists(permissionsFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Group>));
                Groups = (List<Group>)serializer.Deserialize(new StreamReader(permissionsFile));

                foreach (Group group in Groups)
                { 
                    foreach(string command in group.Commands){
                        group.Commands[group.Commands.IndexOf(command)] = command.ToLower();
                    }
                }
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Group>));
                using (TextWriter writer = new StreamWriter(permissionsFile))
                {
                    serializer.Serialize(writer, defaultGroups);
                }
                Groups = defaultGroups;
            }
        }


        public static bool CheckPermissions(SteamPlayer a, string w)
        {
            Regex r = new Regex("^\\/[a-zA-Z]*");
            String commandstring = r.Match(w).Value.ToString().TrimStart('/');

            foreach(Group group in Groups){
                if (
                        a.Admin || 
                        ((group.Name.ToLower() == "default" || group.Members.Contains(a.ToString().ToLower())) && group.Commands.Contains(commandstring.ToLower()))
                    )
                {

                    /*Execute RocketCommand if there is one*/
                    RocketCommand command = Core.Commands.Where(c =>c.Name.ToLower() == commandstring).FirstOrDefault();
                    if (command != null) {
                        command.Execute(a.SteamPlayerId,w);
                        return false;
                    }

                    return true;
                }
            }
            return false;
        }
    }
    public class Group
    {
        public Group() { }
        public Group(string name, List<string> members, List<string> commands)
        {
            Name = name;
            Members = members;
            Commands = commands;
        }
        public string Name;
        [XmlArrayItem(ElementName="Member")]
        public List<string> Members;
        [XmlArrayItem(ElementName = "Command")]
        public List<string> Commands;
    }


}
