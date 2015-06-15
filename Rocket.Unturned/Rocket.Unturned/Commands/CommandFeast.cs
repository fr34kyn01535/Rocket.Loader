using SDG;
using System;
using UnityEngine;
using System.Linq;
using Rocket.Unturned.Logging;
using Rocket.Core;
using Rocket.API;
using Rocket.Core.Translations;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
#if DEBUG
    public class CommandFeast: IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "feast"; }
        }

        public string Help
        {
            get { return "Triggers an effect at your position";}
        }

        public string Syntax
        {
            get { return "<id>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            float i = -100;
            foreach (RocketEffect plane in RocketEffectManager.GetEffectsByType("Plane")) //Get all planes marked as RocketEffect Airdrop
            {
                spawnPlane(plane, caller, i, 75, 300);
                i = i + 50;
            }

            RocketEffect plane1001 = RocketEffectManager.GetEffectsById(1001); //Get RocketEffect with the id 1001
            if (plane1001 != null) //Plane located, workshop content correctly installed
            {
                spawnPlane(plane1001, caller, i, 75, 300);
            }
            else {
                Logger.LogError("plane1001 not found!");
            }

        }

        /// <summary>
        /// Will spawn a plane effect relative to a given player
        /// </summary>
        /// <param name="plane">The RocketEffect containing the plane</param>
        /// <param name="player"></param>
        /// <param name="offset">The horizontal offset to east (negative) and west (positive) to the player position (Note: Planes will always come from north)</param>
        /// <param name="height"></param>
        /// <param name="distance">How far away the plane should start to fly</param>
        private void spawnPlane(RocketEffect plane,RocketPlayer player, float offset,float height,float distance){
            Vector3 position = player.Position;
            position.x += offset; //Ofset
            position.y += height; //Height  
            position.z += distance; //Distance
            plane.Trigger(position);
        }

    }
#endif
}