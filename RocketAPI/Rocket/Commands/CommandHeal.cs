using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandHeal : Command
    {
        public CommandHeal()
        {
            base.commandName = "heal";
            base.commandHelp = "Heals yourself or somebody else";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (!RocketCommand.IsPlayer(caller)) return;

            Player myPlayer = PlayerTool.getPlayer(caller.CSteamID);

            if (String.IsNullOrEmpty(command))
            {
                myPlayer.Heal(100);
                myPlayer.SetBleeding(false);
                myPlayer.SetBroken(false);
                myPlayer.SetInfection(0);
                myPlayer.SetHunger(0);
                myPlayer.SetThirst(0);
                RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_heal_success"));
            }
            else
            {
                Player otherPlayer = PlayerTool.getPlayer(command);
                if (otherPlayer != null && otherPlayer.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID.ToString() != caller.CSteamID.ToString())
                {
                    otherPlayer.Heal(100);
                    otherPlayer.SetBleeding(false);
                    otherPlayer.SetBroken(false);
                    otherPlayer.SetInfection(0);
                    otherPlayer.SetHunger(0);
                    otherPlayer.SetThirst(0);
                    RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_heal_success_me",myPlayer.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
                    RocketChatManager.Say(otherPlayer.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID, RocketTranslation.Translate("command_heal_success_other",caller.CharacterName));
                }
                else
                {
                    RocketChatManager.Say(caller.CSteamID, RocketTranslation.Translate("command_generic_target_player_not_found"));
                }
            }
        }
    }
}