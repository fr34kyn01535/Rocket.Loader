using UnityEngine;

namespace Rocket.Unturned.Player
{
    public class UnturnedPlayerComponent : MonoBehaviour
    {
        private UnturnedPlayer player;
        public UnturnedPlayer Player
        {
            get { return player; }
        }

        private void Awake()
        {
            player = UnturnedPlayer.FromPlayer(gameObject.transform.GetComponent<SDG.Unturned.Player>());
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}