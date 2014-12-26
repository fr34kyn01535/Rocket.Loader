using SDG;
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
        private static string permissionsFile = "./Unturned_Data/Managed/Rocket/groups.config";

        private static List<Group> defaultGroups = new List<Group>() { new Group("default", null, new List<string>() { "plugins", "vote","reward"}) };
        private static List<Group> Groups = null;

        public static void LoadConfiguration()
        {
            if (!Directory.Exists(Path.GetDirectoryName(permissionsFile))) {
                Directory.CreateDirectory(Path.GetDirectoryName(permissionsFile));
            }
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
            String command = r.Match(w).Value.ToString().TrimStart('/');

            foreach(Group group in Groups){
                if (a.Admin || (
                    (group.Name.ToLower() == "default" || group.Players.Contains(a.ToString().ToLower())) &&
                    group.Commands.Contains(command.ToLower())))
                {
                    return true;
                }
            }
            return false;
        }
    }


    public class Group
    {
        public Group() { }
        public Group(string name, List<string> players, List<string> commands)
        {
            Name = name;
            Players = players;
            Commands = commands;
        }
        public string Name;
        public List<string> Players;
        public List<string> Commands;
    }


}
