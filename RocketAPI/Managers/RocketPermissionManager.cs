using Rocket.RocketAPI;
using Rocket.RocketAPI.Components;
using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;

namespace Rocket
{
    public class RocketPermissionManager : RocketManagerComponent
    {
        private string permissionsFile;
        private static Permissions permissions;

        public new void Awake()
        {
            base.Awake();
            permissionsFile = RocketSettings.HomeFolder + "Permissions.config";
            loadPermissions();
        }

        private void loadPermissions()
        {
            if (File.Exists(permissionsFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Permissions));
                permissions = (Permissions)serializer.Deserialize(new StreamReader(permissionsFile));

                foreach (Group group in permissions.Groups)
                {
                    foreach (string command in group.Commands)
                    {
                        group.Commands[group.Commands.IndexOf(command)] = command.ToLower();
                    }
                }
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Permissions));
                using (TextWriter writer = new StreamWriter(permissionsFile))
                {
                    permissions = new Permissions();

                    permissions.ShowGroup = true;
                    permissions.DefaultGroupName = "default";
                    permissions.AdminGroupDisplayName = "Admin";
                    permissions.Format = "[{0}] ";
                    permissions.Groups = new Group[] { 
                            new Group("default","Guest", null , new List<string>() { "reward","balance","pay" }),
                            new Group("moderator","Moderator", new List<string>() { "76561197960287930" }, new List<string>() { "tp", "tphere","i","test" }) 
                        };


                    serializer.Serialize(writer, permissions);
                }
            }
        }

        public static string GetChatPrefix(CSteamID CSteamID)
        {
            string prefix = "";
            try
            {
                if (permissions.ShowGroup)
                {
                    if (PlayerTool.getSteamPlayer(CSteamID).IsAdmin)
                    {
                        return String.Format(permissions.Format, permissions.AdminGroupDisplayName);
                    }
                    else {
                        Group group = permissions.Groups.Where(g => g.Members!= null && g.Members.Contains(CSteamID.ToString())).FirstOrDefault();
                        if (group == null)
                        {
                            Group defaultGroup = permissions.Groups.Where(g => g.Name == permissions.DefaultGroupName).FirstOrDefault();
                            if (defaultGroup == null) throw new Exception("No group found with the name " + permissions.DefaultGroupName + ", can not get default group");
                            return String.Format(permissions.Format, defaultGroup.DisplayName);
                        }
                        else {
                            return String.Format(permissions.Format, group.DisplayName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return prefix;
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

            foreach (Group group in RocketPermissionManager.permissions.Groups)
            {
                if (group.Commands.Contains(commandstring.ToLower()))
                {
                    if(group.Name.ToLower() == permissions.DefaultGroupName) return true;
                    if (group.Members.Contains(player.SteamPlayerID.CSteamID.ToString().ToLower())) return true;
                }
            }

            return player.IsAdmin;
        }
    }

    [Serializable]
    public class Permissions
    {
        public Permissions() { }
        public bool ShowGroup;
        public string DefaultGroupName;
        public string AdminGroupDisplayName;
        public string Format;
        [XmlArrayItem(ElementName = "Group")]
        public Group[] Groups;
    }

    [Serializable]
    public class Group
    {
        public Group() { }
        public Group(string name,string displayName, List<string> members, List<string> commands)
        {
            Name = name;
            DisplayName = displayName;
            Members = members;
            Commands = commands;
        }
        public string Name;
        public string DisplayName;
        [XmlArrayItem(ElementName="Member")]
        public List<string> Members;
        [XmlArrayItem(ElementName = "Command")]
        public List<string> Commands;
    }


}
