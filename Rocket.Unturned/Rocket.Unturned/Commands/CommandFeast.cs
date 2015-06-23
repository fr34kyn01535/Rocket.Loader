using SDG.Unturned;
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
            Console.WriteLine("Spawned Planes");
            foreach (RocketEffect plane in RocketEffectManager.GetEffectsByType("Plane")) //Get all planes marked as RocketEffect Airdrop
            {
                Console.WriteLine(plane.EffectID);
                spawnPlane(plane, caller, i, command.GetFloatParameter(0).Value, command.GetFloatParameter(1).Value);
                i = i + 50;
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