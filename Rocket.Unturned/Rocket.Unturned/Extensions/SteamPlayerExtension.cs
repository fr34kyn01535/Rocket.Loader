using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.Core.Extensions
{
    public static class SteamPlayerExtension
    {
        public static UnturnedPlayer ToUnturnedPlayer(this SDG.Unturned.SteamPlayer player)
        {
            return UnturnedPlayer.FromSteamPlayer(player);
        }
    }
}
