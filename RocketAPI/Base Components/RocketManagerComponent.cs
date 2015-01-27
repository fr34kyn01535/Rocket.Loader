using SDG;
using Steamworks;
using UnityEngine;

namespace Rocket.RocketAPI.Components
{
    public class RocketManagerComponent : MonoBehaviour
    {
        public virtual void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}
