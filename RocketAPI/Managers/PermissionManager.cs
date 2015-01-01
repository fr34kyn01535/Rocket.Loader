using Rocket.RocketAPI.Interfaces;
using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Rocket.RocketAPI.Managers
{
    public class PermissionManager
    {
        private string permissionsFile = Bootstrap.HomeFolder + "Permissions.config";

        private List<Group> defaultGroups = new List<Group>() { new Group("default", new List<string>() { "76561198016438091" }, new List<string>() { "plugins", "vote", "reward" }) };
        internal List<Group> groups = null;

        internal PermissionManager()
        {
            loadPermissions();
        }

        private void loadPermissions()
        {
            if (File.Exists(permissionsFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Group>));
                groups = (List<Group>)serializer.Deserialize(new StreamReader(permissionsFile));

                foreach (Group group in groups)
                {
                    foreach (string command in group.Commands)
                    {
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
                groups = defaultGroups;
            }
        }
        /// <summary>
        /// This method checks if a player has a specific permission
        /// </summary>
        /// <param name="player">The player to check on</param>
        /// <param name="permission">The permission to check</param>
        /// <returns></returns>
        public static bool CheckPermissions(SteamPlayer player, string permission)
        {
            Regex r = new Regex("^\\/[a-zA-Z]*");
            String commandstring = r.Match(permission.ToLower()).Value.ToString().TrimStart('/');

            foreach(Group group in RocketAPI.Permissions.groups){
                if (
                        player.Admin || 
                        ((group.Name.ToLower() == "default" || group.Members.Contains(player.SteamPlayerId.ToString().ToLower())) && group.Commands.Contains(commandstring.ToLower()))
                    )
                {

                    /*Execute RocketCommand if there is one*/
                    RocketCommand command = RocketAPI.Commands.commands.Where(c => c.Name.ToLower() == commandstring).FirstOrDefault();
                    if (command != null) {
                        command.Execute(player.SteamPlayerId, permission);
                        return false;
                    }

                    return true;
                }
            }
            return false;
        }

        internal void Reload()
        {
            loadPermissions();
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
