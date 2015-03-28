using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Commands
{
    public class CommandHeal : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "heal"; }
        }

        public string Help
        {
            get { return "Heals yourself or somebody else";}
        }

        public void Execute(Steamworks.CSteamID caller, string command)
        {
            if (String.IsNullOrEmpty(command) && RocketCommandBase.IsPlayer(caller))
            {
                Player myPlayer = PlayerTool.getPlayer(caller);
                myPlayer.Heal(100);
                myPlayer.SetBleeding(false);
                myPlayer.SetBroken(false);
                myPlayer.SetInfection(0);
                myPlayer.SetHunger(0);
                myPlayer.SetThirst(0);
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_heal_success"));
            }
            else
            {
                Player otherPlayer = PlayerTool.getPlayer(command);
                if (otherPlayer != null && otherPlayer.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID.ToString() != caller.ToString())
                {
                    otherPlayer.Heal(100);
                    otherPlayer.SetBleeding(false);
                    otherPlayer.SetBroken(false);
                    otherPlayer.SetInfection(0);
                    otherPlayer.SetHunger(0);
                    otherPlayer.SetThirst(0);
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_heal_success_me",otherPlayer.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
                    
                    if(RocketCommandBase.IsPlayer(caller))
                        RocketChatManager.Say(otherPlayer.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID, RocketTranslation.Translate("command_heal_success_other", PlayerTool.getPlayer(caller).SteamChannel.SteamPlayer.SteamPlayerID.CharacterName));
                }
                else
                {
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_target_player_not_found"));
                }
            }
        }
    }
}