using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;

namespace Rocket.Unturned.Commands
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

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (caller != null && command.Length != 1)
            {
                caller.Heal(100);
                caller.Bleeding = false;
                caller.Broken = false;
                caller.Infection = 0;
                caller.Hunger = 0;
                caller.Thirst = 0;
                RocketChatManager.Say(caller, RocketTranslation.Translate("command_heal_success"));
            }
            else
            {
                RocketPlayer otherPlayer = RocketPlayer.FromName(command[0]);
                if (otherPlayer != null && otherPlayer != caller)
                {
                    otherPlayer.Heal(100);
                    otherPlayer.Bleeding = false;
                    otherPlayer.Broken = false;
                    otherPlayer.Infection = 0;
                    otherPlayer.Hunger = 0;
                    otherPlayer.Thirst = 0;
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_heal_success_me",otherPlayer.CharacterName));
                    
                    if(caller != null)
                        RocketChatManager.Say(otherPlayer.CSteamID, RocketTranslation.Translate("command_heal_success_other", caller.CharacterName));
                }
                else
                {
                    RocketChatManager.Say(caller, RocketTranslation.Translate("command_generic_target_player_not_found"));
                }
            }
        }
    }
}