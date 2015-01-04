using SDG;
using UnityEngine;

namespace Rocket
{
    public class RocketPlayerComponent : MonoBehaviour
    {
        private Player _player;
        public Player player
        {
            get { return _player; }
            set { _player = value; }
        }

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            player = gameObject.GetComponent<Player>();
            player.PlayerLife.OnDamaged += onDamaged;
            player.PlayerLife.OnUpdateBleeding += onUpdateBleeding;
            player.PlayerLife.OnUpdateBroken += onUpdateBroken;
            player.PlayerLife.OnUpdateFoot += onUpdateFoot;
            player.PlayerLife.OnUpdateFreezing += onUpdateFreezing;
            player.PlayerLife.OnUpdateHealth += onUpdateHealth;
            player.PlayerLife.OnUpdateLife += onUpdateLife;
            player.PlayerLife.OnUpdateOxygen += onUpdateOxygen;
            player.PlayerLife.OnUpdateStamina += onUpdateStamina;
            player.PlayerLife.OnUpdateVirus += onUpdateVirus;
            player.PlayerLife.OnUpdateVision += onUpdateVision;
            player.PlayerLife.OnUpdateWater += onUpdateWater;
        }

        protected virtual void onUpdateBleeding(bool J) { }

        protected virtual void onUpdateBroken(bool D) { }

        protected virtual void onUpdateFoot(byte R) { }

        protected virtual void onUpdateFreezing(bool h) { }

        protected virtual void onUpdateHealth(byte Y) { }

        protected virtual void onUpdateLife(bool C) { }

        protected virtual void onUpdateOxygen(byte t) { }

        protected virtual void onUpdateStamina(byte D) { }

        protected virtual void onUpdateVirus(byte E) { }

        protected virtual void onUpdateVision(bool o) { }

        protected virtual void onUpdateWater(byte A) { }

        protected virtual void onDamaged(byte P) { }
    }
}
