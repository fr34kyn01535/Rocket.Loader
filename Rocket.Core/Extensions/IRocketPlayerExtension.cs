using Rocket.Core;
using System.Collections.Generic;

namespace Rocket.API
{
    public static class IRocketPlayerExtension
    {
        public static bool HasPermission(this IRocketPlayer player, string permission)
        {
            if (player is ConsolePlayer) return true;
            return R.Permissions.HasPermission(player, permission);
        }

        public static List<string> GetPermissions(this IRocketPlayer player)
        {
            if (player is ConsolePlayer) return new List<string>() { "*" };
            return R.Permissions.GetPermissions(player);
        }
    }
}
