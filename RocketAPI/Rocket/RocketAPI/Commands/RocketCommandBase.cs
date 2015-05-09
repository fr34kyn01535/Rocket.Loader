using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Rocket.RocketAPI
{
    internal class RocketCommandBase : Command
    {
        public static bool IsPlayer(Steamworks.CSteamID caller)
        {
            return (caller != null && !String.IsNullOrEmpty(caller.ToString()) && caller.ToString() != "0");
        }

        internal IRocketCommand Command;

        public RocketCommandBase(IRocketCommand command)
        {
            Command = command;
            base.commandName = Command.Name;
            base.commandHelp = Command.Help;
            base.commandInfo = Command.Name + " - " + Command.Help;
        }

        protected override void execute(Steamworks.CSteamID caller, string command)
        {
            if (!Command.RunFromConsole && !IsPlayer(caller))
            {
                Logger.Log("This command can't be called from console");
                return;
            }

            string[] collection = Regex.Matches(command, @"[\""](.+?)[\""]|([^ ]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture).Cast<Match>().Select(m => m.Value.Trim().Trim('"')).ToArray();

            Command.Execute(RocketPlayer.FromCSteamID(caller), collection);
        }
    }
}