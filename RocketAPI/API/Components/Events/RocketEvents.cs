using Rocket.RocketAPI.Components;

namespace Rocket.RocketAPI
{
    public partial class RocketEvents : RocketPlayerComponent
    {
        private new void Awake()
        {
            base.Awake();

            #region RocketPlayerLifeEvents
                PlayerInstance.PlayerLife.OnUpdateStamina += onUpdateStamina;
            #endregion RocketPlayerLifeEvents

            #region RocketPlayerEvents
                PlayerInstance.Inventory.OnInventoryAdded += onInventoryAdded;
                PlayerInstance.Inventory.OnInventoryRemoved += onInventoryRemoved;
                PlayerInstance.Inventory.OnInventoryResized += onInventoryResized;
                PlayerInstance.Inventory.OnInventoryUpdated += onInventoryUpdated;
            #endregion RocketPlayerEvents
        }
    }
}