﻿using Rocket.Core.Permissions;
using Rocket.Core.Settings;
using Rocket.Unturned.Events;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using Rocket.Unturned.Settings;
using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Rocket.Unturned.Permissions
{
    public class RocketPermissions : MonoBehaviour
    {
        private void Awake() { 
            SDG.ChatManager.OnChatted+= handleChat;
        }

        private void handleChat(SteamPlayer steamPlayer, EChatMode chatMode, ref Color incomingColor, string message){
            Color color = Color.white;
            try
            {
                RocketPlayer player = RocketPlayer.FromSteamPlayer(steamPlayer);
                if (player.IsAdmin)
                {
                    color = Palette.Admin;
                }
                else
                {
                    string colorPermission = RocketPermissionsManager.GetPermissions(steamPlayer.ToString()).Where(permission => permission.ToLower().StartsWith("color.")).FirstOrDefault();
                    if (colorPermission != null)
                    {
                        color = RocketChat.GetColorFromName(colorPermission.ToLower().Replace("color.", ""), color);
                    }
                }

                color = RocketPlayerEvents.firePlayerChatted(player, chatMode, color, message);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            incomingColor = color;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]

        public static bool CheckPermissions(SteamPlayer player, string permission)
        {
            Regex r = new Regex("^\\/[a-zA-Z]*");
            String requestedPermission = r.Match(permission.ToLower()).Value.ToString().TrimStart('/').ToLower();

            List<string> permissions = RocketPermissionsManager.GetPermissions(player.SteamPlayerID.CSteamID.ToString());

            if (permissions.Where(p => p.ToLower() == requestedPermission || p.StartsWith(requestedPermission + ".")).Count() != 0 || permissions.Contains("*"))
            {
                return true;
            }

            return player.IsAdmin;
        }

        public static bool HasPermission(SteamPlayer player, string requestedPermission)
        {
            List<string> permissions = RocketPermissionsManager.GetPermissions(player.SteamPlayerID.CSteamID.ToString());

            if (permissions.Where(p => p.ToLower() == requestedPermission || p.ToLower() == requestedPermission.Replace(".",".*") || p.StartsWith(requestedPermission + ".")).Count() != 0 || permissions.Contains("*"))
            {
                return true;
            }

            return player.IsAdmin;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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
            int reservedSlots = ((ImplementationSettings)RocketSettingsManager.Settings.Implementation).ReservedSlots;
            string[] reservedSlotsGroups = ((ImplementationSettings)RocketSettingsManager.Settings.Implementation).ReservedSlotsGroups;

            if (!SteamAdminlist.checkAdmin(cSteamID) && reservedSlotsGroups != null && reservedSlotsGroups.Count() != 0) //If setup
            {
                int maxPlayers = Steam.MaxPlayers;
                int currentPlayers = Steam.Players.Count();

                if (currentPlayers + reservedSlots >= maxPlayers) // If not enought slots
                {
                    string[] myGroups = RocketPermissionsManager.GetGroups(cSteamID.ToString(), true).Select(g => g.Id).ToArray();
                    foreach (string g in myGroups)
                    {
                        if (reservedSlotsGroups.Contains(g))
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
            string[] whitelistGroups = ((ImplementationSettings)RocketSettingsManager.Settings.Implementation).WhitelistedGroups;

            if (!SteamAdminlist.checkAdmin(cSteamID) && (whitelistGroups != null && whitelistGroups.Count() != 0))
            {
                string[] myGroups = RocketPermissionsManager.GetGroups(cSteamID.ToString(), true).Select(g => g.Id).ToArray();
                foreach (string g in myGroups)
                {
                    if (whitelistGroups.Contains(g))
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
}
