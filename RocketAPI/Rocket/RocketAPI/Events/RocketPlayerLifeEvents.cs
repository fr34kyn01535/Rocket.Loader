using Rocket.Components;
using Rocket.Logging;
using SDG;
using Steamworks;
using UnityEngine;

namespace Rocket.RocketAPI.Events
{
    public sealed partial class RocketPlayerEvents : RocketPlayerComponent
    {
        public delegate void PlayerUpdateStamina(SDG.Player player, byte stamina);
        public static event PlayerUpdateStamina OnPlayerUpdateStamina;
        public event PlayerUpdateStamina OnUpdateStamina;

        private void onUpdateStamina(byte stamina)
        {
            try
            {
                if (OnPlayerUpdateStamina != null) OnPlayerUpdateStamina(PlayerInstance, stamina);
                if (OnUpdateStamina != null) OnUpdateStamina(PlayerInstance, stamina);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}