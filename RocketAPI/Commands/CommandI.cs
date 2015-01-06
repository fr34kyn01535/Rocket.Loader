using Rocket.RocketAPI;
using SDG;
using UnityEngine;

namespace Rocket
{
    public class CommandI : Command
    {
        public CommandI() {
            base.commandName = "i";
            base.commandInfo = base.commandHelp = "Gives yourself an item";
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            Logger.Log("cmd:"+command);
            if (command.Length < commandName.Length + 2) return;
            SteamPlayer otherPlayer;
            if (SteamPlayerlist.tryGetSteamPlayer(command.Substring(commandName.Length + 2), out otherPlayer) && otherPlayer.SteamPlayerID.CSteamID.ToString() != caller.CSteamID.ToString())
            {
                SteamPlayer myPlayer = PlayerTool.getSteamPlayer(caller.CSteamID);

                Vector3 d1 = otherPlayer.Player.transform.position;
                Vector3 vector31 = otherPlayer.Player.transform.rotation.eulerAngles;
                myPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
                ChatManager.say(caller.CSteamID, "Teleported to " + otherPlayer.SteamPlayerID.CharacterName);
            }
            else
            {
                ChatManager.say(caller.CSteamID, "Failed to find player");
            }
        }
    }
}
