using Rocket.API;
using Rocket.RocketAPI;
using SDG;
using UnityEngine;

namespace Rocket.API
{
    public class RocketPlayerComponent : MonoBehaviour
    {
        public RocketPlayer Player;

        public void Awake()
        {
            Player = RocketPlayer.FromPlayer(gameObject.transform.GetComponent<Player>());
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}