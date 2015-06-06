using Rocket.Core.Logging;
using Rocket.Core.Settings;
using Rocket.Core.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using UnityEngine;

namespace Rocket.Core.Permissions
{
    public sealed class RocketPermissionsManager : MonoBehaviour
    {
        private static Permissions permissions;

        public void Start()
        {
#if DEBUG
            Logger.Log("RocketPermissionsManager > Awake");
#endif
            Reload();
        }

        private static void loadWebPermissions(string instanceName)
        {
            if (RocketSettingsManager.Settings.WebPermissions.Enabled)
            {
                try
                {
                    WebClient wc = new WebClient();
                    wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                    string target = RocketSettingsManager.Settings.WebPermissions.Url;
                    if (target.Contains("?"))
                    {
                        target += "&";
                    }
                    else
                    {
                        target += "?";
                    }

                    wc.DownloadStringAsync(new Uri(target + "instance=" + instanceName + "&request=" + Guid.NewGuid()));
                    Logger.Log("Updating WebPermissions from " + target);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed getting WebPermissions from " + RocketSettingsManager.Settings.WebPermissions.Url + ": " + ex.ToString());
                }
            }
        }

        private static bool updatedWebPermissions = false;
        private static DateTime lastUpdatedWebPermissions = DateTime.MinValue;

        private void FixedUpdate()
        {
            if (updatedWebPermissions && RocketSettingsManager.Settings.WebPermissions.Interval > 0 && (DateTime.Now - lastUpdatedWebPermissions) > TimeSpan.FromSeconds(RocketSettingsManager.Settings.WebPermissions.Interval))
            {
                updatedWebPermissions = false;
                Reload();
            }
        }

        public static void Reload(bool writeAgain = true)
        {
            if (RocketSettingsManager.Settings.WebPermissions.Enabled)
            {
                loadWebPermissions(RocketBootstrap.Implementation.InstanceName);
            }
            else
            {
                loadPermissions(RocketBootstrap.Implementation.ConfigurationFolder + RocketBootstrap.PermissionFile, writeAgain);
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
                        Logger.LogError("Failed getting WebPermissions from " + RocketSettingsManager.Settings.WebPermissions.Url + ": Empty result");
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
                r = "Failed getting WebPermissions from " + RocketSettingsManager.Settings.WebPermissions.Url + ": " + ex.ToString();
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

        private static void loadPermissions(string permissionsFile,bool writeAgain = false)
        {
            try
            {
                if (File.Exists(permissionsFile))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Permissions));
                    using (StreamReader reader = new StreamReader(permissionsFile))
                    {
                        permissions = (Permissions)serializer.Deserialize(reader);
                        if (permissions.Groups == null) permissions.Groups = new Group[0];
                        if (String.IsNullOrEmpty(permissions.DefaultGroupId)) permissions.DefaultGroupId = "default";
                    }
                    if (writeAgain)
                    {
                        using (StreamWriter streamWriter = new StreamWriter(permissionsFile))
                        {
                            serializer.Serialize(streamWriter, permissions);
                        }
                    }
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Permissions));
                    using (TextWriter writer = new StreamWriter(permissionsFile))
                    {
                        permissions = new Permissions();

                        permissions.DefaultGroupId = "default";
                        permissions.Groups = new Group[] {
                            new Group("default","Guest",new List<string>(), null , new List<string>() { "p", "reward","balance","pay","rocket","color.white" }),
                            new Group("moderator","Moderator", new List<string>(),new List<string>() { "76561197960287930" }, new List<string>() { "p", "p.reload", "tp", "tphere","i","test" })
                        };
                        serializer.Serialize(writer, permissions);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.LogError("Failed loading the permissions, check if the XML is not malformed: "+ex.ToString());
            }
        }

        public static void SavePermissions(string permissionsFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Permissions));
            using (TextWriter writer = new StreamWriter(permissionsFile))
            {
                serializer.Serialize(writer, permissions);
            }
        }

        public static string[] GetDisplayGroups(string userID)
        {
            return GetGroups(userID, true).Select(g => g.DisplayName + " (" + g.Id + ")").ToArray();
        }

        private static List<Group> getGroupsByIds(List<string> ids)
        {
            return permissions.Groups.Where(g => ids.Select(i => i.ToLower()).Contains(g.Id.ToLower())).ToList();
        }


        private static List<string> getParentGroups(List<string> parentGroups, string currentGroup)
        {
            List<string> allgroups = new List<string>();
            foreach (string g in parentGroups)
            {
                Group group = permissions.Groups.Where(gr => gr.Id.ToLower() == g.ToLower()).FirstOrDefault();
                if (group != null && currentGroup.ToLower() != group.Id.ToLower())
                {
                    allgroups.Add(group.Id);
                    allgroups.AddRange(getParentGroups(group.ParentGroups, currentGroup));
                }
            }
            return allgroups;
        }


        public static List<Group> GetGroups(string userID, bool includeParentGroups)
        {
            List<Group> groups = permissions.Groups.Where(g => g.Members.Contains(userID)).ToList(); // Get my Groups
            Group defaultGroup = permissions.Groups.Where(g => g.Id.ToLower() == permissions.DefaultGroupId.ToLower()).FirstOrDefault();
            if (defaultGroup != null) groups.Add(defaultGroup);

            if (includeParentGroups)
            {
                List<Group> parentGroups = new List<Group>();
                foreach (Group g in groups)
                {
                    parentGroups.AddRange(getGroupsByIds(getParentGroups(g.ParentGroups, g.Id)));
                }
                groups.AddRange(parentGroups);
            }

            return groups.Distinct().ToList();
        }

        public static List<string> GetPermissions(string userID)
        {
            List<string> p = new List<string>();

            List<Group> myGroups = GetGroups(userID, true);

            foreach (Group g in myGroups)
            {
                foreach (Group myGroup in permissions.Groups.Where(group => group.ParentGroups.Select(parentGroup => parentGroup.ToLower()).Contains(g.Id.ToLower())))
                {
                    if (myGroup.Members.Contains(userID.ToString()))
                        p.AddRange(myGroup.Commands);
                }
                p.AddRange(g.Commands);
            }

            return p.Distinct().ToList();
        }

        public static bool SetGroup(string player, string groupName)
        {
            bool added = false;
            foreach (Group g in permissions.Groups)
            {
                if (g.Members.Contains(player))
                {
                    g.Members.Remove(player);
                }
                if (g.Id.ToLower() == groupName.ToLower())
                {
                    g.Members.Add(player);
                    added = true;
                }
            }
            if (added)
            {
                SavePermissions(RocketBootstrap.Implementation.ConfigurationFolder + RocketBootstrap.PermissionFile);
                return true;
            }
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
    }


    [Serializable]
    public class Group
    {
        public Group()
        {
        }

        public Group(string name, string displayName, List<string> parentGroups, List<string> members, List<string> commands)
        {
            Id = name;
            DisplayName = displayName;
            Members = members;
            Commands = commands;
            ParentGroups = parentGroups;
        }

        public string Id;
        public string DisplayName;

        [XmlArrayItem(ElementName = "Member")]
        public List<string> Members;

        [XmlArrayItem(ElementName = "Command")]
        public List<string> Commands;

        [XmlArrayItem(ElementName = "ParentGroup")]
        public List<string> ParentGroups;
    }
}