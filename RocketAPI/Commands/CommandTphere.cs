using SDG;
using UnityEngine;

namespace Rocket
{
    class CommandTphere : Command
    {
        public CommandTphere()
        {
            base.commandName = "tphere";
            base.commandInfo = base.commandHelp = "Teleports another player to you";
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (command.Length < commandName.Length + 2) return;
            SteamPlayer otherPlayer;
            if (SteamPlayerlist.tryGetSteamPlayer(command.Substring(commandName.Length + 2), out otherPlayer) && otherPlayer.SteamPlayerID.CSteamId.ToString() != caller.CSteamId.ToString())
            {
                SteamPlayer myPlayer = PlayerTool.getSteamPlayer(caller.CSteamId);

                Vector3 d1 = myPlayer.Player.transform.position;
                Vector3 vector31 = myPlayer.Player.transform.rotation.eulerAngles;
                otherPlayer.Player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
                Chat.Say(caller.CSteamId, "Teleported " + otherPlayer.SteamPlayerID.CharacterName + " to you");
                Chat.Say(otherPlayer.SteamPlayerID.CSteamId, "You were teleported to " + myPlayer.SteamPlayerID.CharacterName);
            }
            else
            {
                Chat.Say(caller.CSteamId, "Failed to find player");
            }

        }
    }
}
