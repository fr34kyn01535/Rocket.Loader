using Rocket.RocketAPI;
using SDG;
using UnityEngine;

namespace Rocket
{
    class CommandTphere : Command
    {
        public CommandTphere()
        {
            base.commandName = "tphere";
            base.commandHelp = "Teleports another player to you";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            SteamPlayer otherPlayer;
            if (SteamPlayerlist.tryGetSteamPlayer(command, out otherPlayer) && otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.CSteamID.ToString())
            {
                SteamPlayer myPlayer = PlayerTool.getSteamPlayer(caller.CSteamID);

                Vector3 d1 = myPlayer.Player.transform.position;
                Vector3 vector31 = myPlayer.Player.transform.rotation.eulerAngles;
                otherPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
                ChatManager.say(caller.CSteamID, "Teleported " + otherPlayer.SteamPlayerID.CharacterName + " to you");
                ChatManager.say(otherPlayer.SteamPlayerID.CSteamID, "You were teleported to " + myPlayer.SteamPlayerID.CharacterName);
            }
            else
            {
                ChatManager.say(caller.CSteamID, "Failed to find player");
            }
        }
    }
}
