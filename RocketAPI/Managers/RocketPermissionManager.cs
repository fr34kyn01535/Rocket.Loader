using Rocket.RocketAPI;
using Rocket.RocketAPI.Components;
using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public static DateTime lastUpdate = DateTime.MinValue;

        public new void Awake()
        {
            base.Awake();
            permissionsFile = RocketSettings.HomeFolder + "Permissions.config";
            loadPermissions();
        }

        private static void getWebPermissions()
        {
            try
            {
                if (!String.IsNullOrEmpty(permissions.WebPermissionsUrl) && (DateTime.Now - lastUpdate) > TimeSpan.FromSeconds(permissions.WebCacheTimeout))
                {
                    WebClient wc = new WebClient();
                    wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                    wc.DownloadStringAsync(new Uri(permissions.WebPermissionsUrl));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed getting WebPermissions: " + ex.ToString());
            }
        }

        private static string wc_DownloadStringCompletedDone = null;
        private static void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string r;
            try
            {
                var serializer = new XmlSerializer(typeof(Permissions));
                Permissions result;

                using (TextReader reader = new StringReader(e.Result))
                {
                    result = (Permissions)serializer.Deserialize(reader);
                }
                permissions.Groups = result.Groups;
                r = "Updated WebPermissions...";
            }
            catch (Exception ex)
            {
                r = "Failed downloading WebPermissions: "+ex.ToString();
            }

            wc_DownloadStringCompletedDone = r;
        }

        void Update()
        {
            if (wc_DownloadStringCompletedDone != null)
            {
                Logger.LogWarning(wc_DownloadStringCompletedDone);
                wc_DownloadStringCompletedDone = null;
            }
        }

        private void loadPermissions()
        {
            if (File.Exists(permissionsFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Permissions));
                using (StreamReader reader = new StreamReader(permissionsFile))
                {
                    permissions = (Permissions)serializer.Deserialize(reader);

                    if (permissions.Groups == null) permissions.Groups = new Group[0];

                    foreach (Group group in permissions.Groups)
                    {
                        foreach (string command in group.Commands)
                        {
                            group.Commands[group.Commands.IndexOf(command)] = command.ToLower();
                        }
                    }
                }
                serializer.Serialize(new StreamWriter(permissionsFile), permissions);

                getWebPermissions();
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
                    permissions.WebPermissionsUrl = " ";
                    permissions.WebCacheTimeout = 60;

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

        public static string[] GetGroups(CSteamID CSteamID)
        {
            return permissions.Groups.Where(g => g.Members.Contains(CSteamID.ToString())).Select(g => g.DisplayName + " (" + g.Name + ")").ToArray();
        }
        public static string[] GetPermissions(CSteamID CSteamID)
        {
            List<string> p = new List<string>();
            foreach (Group g in permissions.Groups) {
                if (g.Members.Contains(CSteamID.ToString()) || g.Name == permissions.DefaultGroupName)
                {
                    p.AddRange(g.Commands);
                }
            }
            return p.Distinct().ToArray();
        }

        /// <summary>
        /// This method checks if a player has a specific permission
        /// </summary>
        /// <param name="player">The player to check on</param>
        /// <param name="permission">The permission to check</param>
        /// <returns></returns>
        public static bool CheckPermissions(SteamPlayer player, string permission)
        {
            getWebPermissions();
            Regex r = new Regex("^\\/[a-zA-Z]*");
            String commandstring = r.Match(permission.ToLower()).Value.ToString().TrimStart('/');

            foreach (Group group in RocketPermissionManager.permissions.Groups)
            {
                if (group.Commands.Contains(commandstring.ToLower()) || group.Commands.Contains("*"))
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
        public string WebPermissionsUrl;
        public int WebCacheTimeout;
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
