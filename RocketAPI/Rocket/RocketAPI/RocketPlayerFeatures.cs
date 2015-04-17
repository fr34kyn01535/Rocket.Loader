using Rocket.Components;
using Rocket.Logging;
using Rocket.RocketAPI.Events;
using SDG;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public sealed class RocketPlayerFeatures : RocketPlayerComponent
    {
        private RocketPlayer pl = null;
        private bool godMode = false;

        public bool VanishMode {
            get { return vanishMode; }
            set { vanishMode = value; }
        }

        private bool vanishMode = false;

        public bool GodMode
        {
            set
            {
                if (value)
                {
                    pl.Events.OnUpdateHealth += e_OnPlayerUpdateHealth;
                    pl.Events.OnUpdateWater += e_OnPlayerUpdateWater;
                    pl.Events.OnUpdateFood += e_OnPlayerUpdateFood;
                    pl.Events.OnUpdateVirus += e_OnPlayerUpdateVirus;
                }
                else
                {
                    pl.Events.OnUpdateHealth -= e_OnPlayerUpdateHealth;
                    pl.Events.OnUpdateWater -= e_OnPlayerUpdateWater;
                    pl.Events.OnUpdateFood -= e_OnPlayerUpdateFood;
                    pl.Events.OnUpdateVirus -= e_OnPlayerUpdateVirus;
                }
                godMode = value;
            }
            get
            {
                return godMode;
            }
        }


        public void FixedUpdate()
        {
            if (this.vanishMode)
            {
                pl.Player.SteamChannel.send("tellPosition", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UDP_BUFFER, new object[] {new Vector3(pl.Position.x,-3,pl.Position.z)});
            }
        }

        private void Start()
        {
            pl = RocketPlayer.FromPlayer(gameObject.transform.GetComponent<Player>());
            

            if (godMode)
            {
                pl.Events.OnUpdateHealth += e_OnPlayerUpdateHealth;
                pl.Events.OnUpdateWater += e_OnPlayerUpdateWater;
                pl.Events.OnUpdateFood += e_OnPlayerUpdateFood;
                pl.Events.OnUpdateVirus += e_OnPlayerUpdateVirus;
                pl.Heal(100);
                pl.Infection = 0;
                pl.Hunger = 0;
                pl.Thirst = 0;
                pl.Bleeding = false;
                pl.Broken = false;
            }
        }

        private void e_OnPlayerUpdateVirus(RocketPlayer player, byte virus)
        {
            if (virus < 95) pl.Infection = 0;
        }

        private void e_OnPlayerUpdateFood(RocketPlayer player, byte food)
        {
            if (food < 95) pl.Hunger = 0;
        }

        private void e_OnPlayerUpdateWater(RocketPlayer player, byte water)
        {
            if (water < 95) pl.Thirst = 0;
        }

        private void e_OnPlayerUpdateHealth(RocketPlayer player, byte health)
        {
            if (health < 95)
            {
                pl.Heal(100); 
                pl.Bleeding = false;
                pl.Broken = false;
            }
        }
    }
}