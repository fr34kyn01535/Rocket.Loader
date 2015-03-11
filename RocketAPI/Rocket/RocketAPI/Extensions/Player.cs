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


        public static bool GetBroken(this SDG.Player player)
        {
            return player.PlayerLife.Broken;
        }

        public static void SetBroken(this SDG.Player player, bool broken)
        {
            player.PlayerLife.SteamChannel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { broken });
        }
        public static bool GetBleeding(this SDG.Player player)
        {
            return player.PlayerLife.Bleeding;
        }

        public static void SetBleeding(this SDG.Player player, bool bleeding)
        {
            player.PlayerLife.Bleeding = bleeding;
            player.PlayerLife.SteamChannel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { bleeding });
        }

        public static bool GetDead(this SDG.Player player)
        {
            return player.PlayerLife.Dead;
        }

        public static bool GetFreezing(this SDG.Player player)
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
