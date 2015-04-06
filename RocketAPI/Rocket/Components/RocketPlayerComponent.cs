using Rocket.RocketAPI;
using SDG;
using UnityEngine;

namespace Rocket.Components
{
    public class RocketPlayerComponent : MonoBehaviour
    {
        public RocketPlayer PlayerInstance;

        public void Awake()
        {
            PlayerInstance = RocketPlayer.FromPlayer(gameObject.transform.GetComponent<Player>());
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}