using Rocket.Core;

namespace Rocket.API
{
    public static class IRocketPlayerExtension
    {
        public static bool HasPermission(this IRocketPlayer player,string permission)
        {
            if (player is ConsolePlayer) return true;
            return R.Permissions.HasPermission(player, permission);
        }
    }
}
