using Rocket.RocketAPI.Components;
using SDG;
using Steamworks;
using System;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public class RocketPlayer : RocketPlayerComponent
    {
        public void Damage(byte amount,Vector3 direction,EDeathCause cause,ELimb limb,CSteamID damager){
            PlayerInstance.PlayerLife.askDamage(amount,direction,cause,limb,damager);
        }

        public void Suicide()
        {
            PlayerInstance.PlayerLife.askSuicide(PlayerInstance.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID);
        }

        public byte Health { 
            get{
                return PlayerInstance.PlayerLife.Health;
            }
        }

        public void Heal(byte health) {
            PlayerInstance.PlayerLife.askHeal(health, PlayerInstance.PlayerLife.Bleeding, PlayerInstance.PlayerLife.Broken);
        }

        public byte Hunger
        {
            get
            {
                return PlayerInstance.PlayerLife.Hunger;
            }
            set
            {
                PlayerInstance.PlayerLife.askEat(100);
                PlayerInstance.PlayerLife.askStarve(value);
            }
        }

        public byte Thirst
        {
            get
            {
                return PlayerInstance.PlayerLife.Thirst;
            }
            set
            {
                PlayerInstance.PlayerLife.askDrink(100);
                PlayerInstance.PlayerLife.askDehydrate(value);
            }
        }

        public bool Bleeding
        {
            get
            {
                return PlayerInstance.PlayerLife.Bleeding;
            }
            set
            {
                PlayerInstance.PlayerLife.Bleeding = value;
                PlayerInstance.PlayerLife.SteamChannel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { value });
            }
        }

        public bool Broken
        {
            get
            {
                return PlayerInstance.PlayerLife.Broken;
            }
            set
            {
               // PlayerInstance.PlayerLife.Broken = value;
                PlayerInstance.PlayerLife.SteamChannel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { value });
            }
        }

        /* TODO: Implement
        public byte Breath
        {
            get
            {
                return PlayerInstance.PlayerLife.Breath;
            }
            set
            {
                PlayerInstance.PlayerLife.askBreath(100);
                PlayerInstance.PlayerLife.askSuffocate(value);
            }
        }*/

        public byte Infection
        {
            get
            {
                return PlayerInstance.PlayerLife.Infection;
            }
            set
            {
                PlayerInstance.PlayerLife.askDisinfect(100);
                PlayerInstance.PlayerLife.askInfect(value);
            }
        }

        public byte Stamina
        {
            get
            {
                return PlayerInstance.PlayerLife.Stamina;
            }
            /*set{
               TODO: REST & TIRE
            }*/
        }

        public bool IsDeath
        {
            get
            {
                return PlayerInstance.PlayerLife.Dead;
            }
        }

        public bool IsFreezing
        {
            get
            {
                return PlayerInstance.PlayerLife.Freezing;
            }
        }
    
    }
}
