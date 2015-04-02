using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public static class PlayerExtensions
    {
        public static List<Group> GetGroups(this SDG.Player player)
        {
            return RocketPermissionManager.GetGroups(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID);
        }

        public static List<string> GetPermissions(this SDG.Player player)
        {
            return RocketPermissionManager.GetPermissions(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID);
        }

        public static void Kick(this SDG.Player player, string reason)
        {
            Steam.kick(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID, reason);
        }

        public static void Ban(this SDG.Player player, string reason, uint duration)
        {
            Steam.ban(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID, reason, duration);
        }

        public static bool IsAdmin(this SDG.Player player)
        {
            return player.SteamChannel.SteamPlayer.IsAdmin;
        }
        
        public static void SetAdmin(this SDG.Player player,bool admin,SDG.Player issuer = null)
        {
            if (admin)
            {
                if (issuer == null)
                {
                    SteamAdminlist.admin(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID, new CSteamID(0));
                }
                else
                {
                    SteamAdminlist.admin(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID, issuer.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID);
                }
            }
            else {
                SteamAdminlist.unadmin(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID);
            }
        } 

        public static void Teleport(this SDG.Player player, SDG.Player target)
        {
            Vector3 d1 = target.transform.position;
            Vector3 vector31 = target.transform.rotation.eulerAngles;
            player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
        }

        public static bool Teleport(this SDG.Player player, string node)
        {
            Node item = LevelNodes.Nodes.Where(n => n.NodeType == ENodeType.Location && ((NodeLocation)n).Name.ToLower().Contains(node)).FirstOrDefault();
            if (item != null)
            {
                Vector3 c = item.Position + new Vector3(0f, 0.5f, 0f);
                Vector3 vector31 = player.transform.rotation.eulerAngles;
                player.sendTeleport(c, MeasurementTool.angleToByte(vector31.y));
                return true;
            }
            return false;
        }

        public static byte GetStamina(this SDG.Player player)
        {
            return player.PlayerLife.Stamina;
        }

        public static byte GetInfection(this SDG.Player player)
        {
            return player.PlayerLife.Infection;
        }

        public static void SetInfection(this SDG.Player player, byte infection)
        {
            player.PlayerLife.askDisinfect(100);
            player.PlayerLife.askInfect(infection);
        }

        public static byte GetHealth(this SDG.Player player)
        {
            return player.PlayerLife.Health;
        }

        public static byte GetHunger(this SDG.Player player)
        {
            return player.PlayerLife.Hunger;
        }

        public static void SetHunger(this SDG.Player player, byte hunger)
        {
            player.PlayerLife.askEat(100);
            player.PlayerLife.askStarve(hunger);
        }

        public static byte GetThirst(this SDG.Player player)
        {
            return player.PlayerLife.Thirst;
        }

        public static void SetThirst(this SDG.Player player, byte thirst)
        {
            player.PlayerLife.askDrink(100);
            player.PlayerLife.askDehydrate(thirst);
        }

        public static bool IsBroken(this SDG.Player player)
        {
            return player.PlayerLife.Broken;
        }

        public static void SetBroken(this SDG.Player player, bool broken)
        {
            player.PlayerLife.SteamChannel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { broken });
        }
        public static bool IsBleeding(this SDG.Player player)
        {
            return player.PlayerLife.Bleeding;
        }

        public static void SetBleeding(this SDG.Player player, bool bleeding)
        {
            player.PlayerLife.Bleeding = bleeding;
            player.PlayerLife.SteamChannel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { bleeding });
        }

        public static bool IsDead(this SDG.Player player)
        {
            return player.PlayerLife.Dead;
        }

        public static bool IsFreezing(this SDG.Player player)
        {
            return player.PlayerLife.Freezing;
        }

        public static void Heal(this SDG.Player player, byte amount, bool? bleeding = null, bool? broken = null)
        {
            player.PlayerLife.askHeal(amount, bleeding != null ? bleeding.Value : player.PlayerLife.Bleeding, broken != null ? broken.Value : player.PlayerLife.Broken);
        }

        public static void Suicide(this SDG.Player player)
        {
            player.PlayerLife.askSuicide(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID);
        }

        public static void Damage(this SDG.Player player, byte amount, Vector3 direction, EDeathCause cause, ELimb limb, CSteamID damageDealer)
        {
            player.PlayerLife.askDamage(amount, direction, cause, limb, damageDealer);
        }

    }
}
