using Rocket.Components;
using Rocket.Logging;
using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Rocket.RocketAPI
{
    public sealed class RocketPermissionManager : RocketManagerComponent
    {
        private static string permissionsFile;
        private static Permissions permissions;

        public new void Awake()
        {
#if DEBUG
            Logger.Log("Awake RocketPermissionManager");
#endif
            base.Awake();
            permissionsFile = RocketSettings.HomeFolder + "Permissions.config.xml";
            ReloadPermissions();
        }

        private static void loadWebPermissions()
        {
            if (!String.IsNullOrEmpty(RocketSettings.WebPermissions))
            {
                try
                {
                    RocketWebClient wc = new RocketWebClient();
                    wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                    string target = RocketSettings.WebPermissions;
                    if (target.Contains("?"))
                    {
                        target += "&";
                    }
                    else
                    {
                        target += "?";
                    }

                    wc.DownloadStringAsync(new Uri(target +"instance=" + Steam.InstanceName +"&request=" + Guid.NewGuid()));
                    Logger.Log("Updating WebPermissions from " + target);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed getting WebPermissions from " + RocketSettings.WebPermissions + ": " + ex.ToString());
                }
            }
        }

        private static bool updatedWebPermissions = false;
        private static DateTime lastUpdatedWebPermissions = DateTime.MinValue;

        private void FixedUpdate()
        {
            if (updatedWebPermissions && RocketSettings.WebPermissionsUpdateInterval > 0 && (DateTime.Now - lastUpdatedWebPermissions) > TimeSpan.FromSeconds(RocketSettings.WebPermissionsUpdateInterval))
            {
                updatedWebPermissions = false;
                ReloadPermissions();
            }
        }

        internal static void ReloadPermissions() {
            if (String.IsNullOrEmpty(RocketSettings.WebPermissions))
            {
                loadPermissions();
            }
            else
            {
                loadWebPermissions();
            }
        }

        private static void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string r = null;
            string re = e.Result;

            try
            {
                var serializer = new XmlSerializer(typeof(Permissions));
                Permissions result;
                if (String.IsNullOrEmpty(re))
                {
                    RocketTaskManager.Enqueue(() =>
                    {
                        Logger.LogError("Failed getting WebPermissions from " + RocketSettings.WebPermissions + ": Empty result");
                    });
                }
                else
                {
                    using (StringReader reader = new StringReader(re))
                    {
                        result = (Permissions)serializer.Deserialize(reader);
                    }
                    permissions = result;
                }
            }
            catch (Exception ex)
            {
                r = "Failed getting WebPermissions from " + RocketSettings.WebPermissions + ": " + ex.ToString();
                if (!String.IsNullOrEmpty(r))
                {
                    r += " Result:" + r;
                }
            }

            RocketTaskManager.Enqueue(() =>
            {
                if (!String.IsNullOrEmpty(r)) Logger.LogError(r);
            });
            lastUpdatedWebPermissions = DateTime.Now;
            updatedWebPermissions = true;
        }

        private static void loadPermissions()
        {
            if (File.Exists(permissionsFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Permissions));
                using (StreamReader reader = new StreamReader(permissionsFile))
                {
                    permissions = (Permissions)serializer.Deserialize(reader);

                    if (permissions.Groups == null) permissions.Groups = new Group[0];
                    if (String.IsNullOrEmpty(permissions.DefaultGroupId)) permissions.DefaultGroupId = "default";

                    foreach (Group group in permissions.Groups)
                    {
                        foreach (string command in group.Commands)
                        {
                            group.Commands[group.Commands.IndexOf(command)] = command.ToLower();
                        }
                    }
                }
                serializer.Serialize(new StreamWriter(permissionsFile), permissions);
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Permissions));
                using (TextWriter writer = new StreamWriter(permissionsFile))
                {
                    permissions = new Permissions();

                    permissions.DefaultGroupId = "default";
                    permissions.Groups = new Group[] {
                            new Group("default","Guest", null , new List<string>() { "p", "reward","balance","pay","rocket" }),
                            new Group("moderator","Moderator", new List<string>() { "76561197960287930" }, new List<string>() { "p", "p.reload", "tp", "tphere","i","test" })
                        };
                    permissions.WhitelistedGroups = new string[0];
                    permissions.ReservedSlotsGroups = new string[0];
                    serializer.Serialize(writer, permissions);
                }
            }
        }

        public static void SavePermissions()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Permissions));
            using (TextWriter writer = new StreamWriter(permissionsFile))
            {
                serializer.Serialize(writer, permissions);
            }
        }

        public static string[] GetWhitelistedGroups() { 
            return permissions.WhitelistedGroups;
        }

        //public static string GetChatPrefix(CSteamID CSteamID)
        //{
        //    string prefix = "";
        //    try
        //    {
        //        if (permissions.ShowGroup)
        //        {
        //            if (PlayerTool.getSteamPlayer(CSteamID).IsAdmin)
        //            {
        //                return String.Format(permissions.Format, permissions.AdminGroupDisplayName);
        //            }
        //            else
        //            {
        //                Group group = permissions.Groups.Where(g => g.Members != null && g.Members.Contains(CSteamID.ToString())).FirstOrDefault();
        //                if (group == null)
        //                {
        //                    Group defaultGroup = permissions.Groups.Where(g => g.Id == permissions.DefaultGroupId).FirstOrDefault();
        //                    if (defaultGroup == null) throw new Exception("No group found with the name " + permissions.DefaultGroupId + ", can not get default group");
        //                    return String.Format(permissions.Format, defaultGroup.DisplayName);
        //                }
        //                else
        //                {
        //                    return String.Format(permissions.Format, group.DisplayName);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(ex);
        //    }
        //    return prefix;
        //}

        public static string[] GetDisplayGroups(CSteamID CSteamID)
        {
            return permissions.Groups.Where(g => g.Members.Contains(CSteamID.ToString())).Select(g => g.DisplayName + " (" + g.Id + ")").ToArray();
        }

        public static List<Group> GetGroups(CSteamID CSteamID)
        {
            return permissions.Groups.Where(g => g.Members.Contains(CSteamID.ToString())).ToList();
        }

        public static List<string> GetPermissions(CSteamID CSteamID)
        {
            List<string> p = new List<string>();
            foreach (Group g in permissions.Groups)
            {
                if (g.Members.Contains(CSteamID.ToString()) || g.Id == permissions.DefaultGroupId)
                {
                    p.AddRange(g.Commands);
                }
            }
            return p.Distinct().ToList();
        }

        public static bool CheckPermissions(SteamPlayer player, string permission)
        {
            Regex r = new Regex("^\\/[a-zA-Z]*");
            String commandstring = r.Match(permission.ToLower()).Value.ToString().TrimStart('/');

            foreach (Group group in RocketPermissionManager.permissions.Groups)
            {
                if (group.Commands.Where(c => c.ToLower() == commandstring.ToLower() || c.StartsWith(commandstring.ToLower() + ".")).Count() != 0 || group.Commands.Contains("*"))
                {
                    if (group.Id.ToLower() == permissions.DefaultGroupId) return true;
                    if (group.Members.Contains(player.SteamPlayerID.CSteamID.ToString().ToLower())) return true;
                }
            }

            return player.IsAdmin;
        }

        public static int GetProtectedSlots() {
            return permissions.ReservedSlots;
        }



        public static bool CheckValid(ValidateAuthTicketResponse_t r)
        {
            if (!checkReservedSlotSpace(r.m_SteamID) || !checkWhitelisted(r.m_SteamID))
            {
                return false;
            }
            return true;
        }

        private static bool checkReservedSlotSpace(CSteamID cSteamID)
        {
            if (!SteamAdminlist.checkAdmin(cSteamID) && permissions.ReservedSlotsGroups != null && permissions.ReservedSlotsGroups.Count() != 0 && permissions.Groups != null && permissions.Groups.Count() != 0) //If setup
            {
                int maxPlayers = Steam.MaxPlayers;
                int currentPlayers = Steam.Players.Count();
                int reservedSlots = permissions.ReservedSlots;

                if (currentPlayers + reservedSlots >= maxPlayers) // If not enought slots
                {

                    string[] myGroups = permissions.Groups.Where(g => g.Members.Contains(cSteamID.ToString())).Select(g => g.Id).ToArray();
                    foreach (string g in myGroups)
                    {
                        if (permissions.ReservedSlotsGroups.Contains(g))
                        {
                            return true;
                        }
                    }
                    Steam.Reject(cSteamID, ESteamRejection.SERVER_FULL);
                    return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        private static bool checkWhitelisted(CSteamID cSteamID)
        {
            if (!SteamAdminlist.checkAdmin(cSteamID) && (permissions.WhitelistedGroups != null && permissions.WhitelistedGroups.Count() != 0 && permissions.Groups != null && permissions.Groups.Count() != 0)) 
            {
                string[] myGroups = permissions.Groups.Where(g => g.Members.Contains(cSteamID.ToString())).Select(g => g.Id).ToArray();
                foreach (string g in myGroups)
                {
                    if (permissions.WhitelistedGroups.Contains(g))
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
            Steam.Reject(cSteamID, ESteamRejection.WHITELISTED);
            return false;
        }
    }

    [Serializable]
    public class Permissions
    {
        public Permissions()
        {
        }

        public string DefaultGroupId;
        
        [XmlArrayItem(ElementName = "Group")]
        public Group[] Groups;

        [XmlArrayItem(ElementName = "WhitelistedGroup")]
        public string[] WhitelistedGroups;

        [XmlArrayItem(ElementName = "ReservedSlotsGroup")]
        public string[] ReservedSlotsGroups;
        public int ReservedSlots = 0;
    }
    

    [Serializable]
    public class Group
    {
        public Group()
        {
        }

        public Group(string name, string displayName, List<string> members, List<string> commands)
        {
            Id = name;
            DisplayName = displayName;
            Members = members;
            Commands = commands;
        }

        public string Id;
        public string DisplayName;

        [XmlArrayItem(ElementName = "Member")]
        public List<string> Members;

        [XmlArrayItem(ElementName = "Command")]
        public List<string> Commands;
    }
}