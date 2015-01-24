using Rocket.RocketAPI.Components;
using SDG;
using Steamworks;
using System;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public partial class RocketPlayer : RocketPlayerComponent
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
           // set
            //{
                //Decrease life is probably only possible with damage, having more then 20 life after damage will make u bleed tho
               // PlayerInstance.PlayerLife.askDamage((byte)(PlayerInstance.PlayerLife.Health-15), Vector3.down, EDeathCause.KILL, ELimb.SKULL, new CSteamID(0));
                //PlayerInstance.PlayerLife.askHeal((byte)(value - 15), PlayerInstance.PlayerLife.Bleeding, PlayerInstance.PlayerLife.Broken);
           // }
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
            /*set
            {
                //TODO: Make visible
                //PlayerInstance.PlayerLife.askHeal(PlayerInstance.PlayerLife.Health, value, PlayerInstance.PlayerLife.Broken);
                //Player.Instance.PlayerLife.askDamage(1)

                PlayerInstance.PlayerLife.SteamChannel.send("tellBleeding", ESteamCall.SERVER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { value });
            }*/
        }

        public bool Broken
        {
            get
            {
                return PlayerInstance.PlayerLife.Broken;
            }
            /*set
            { TODO: FIX
                PlayerInstance.PlayerLife.SteamChannel.send("tellBroken", ESteamCall.SERVER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { value });
            }*/
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
               TODO: REST
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
