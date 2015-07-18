using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Misc;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rocket.Unturned.Plugins
{
    public sealed class PluginUnturnedPlayerComponentManager : MonoBehaviour
    {
        private Assembly assembly = Assembly.GetExecutingAssembly();
        private List<Type> UnturnedPlayerComponents = new List<Type>();
        
        private void OnDisable()
        {
            try
            {
                U.Events.OnPlayerConnected -= addPlayerComponents;
                UnturnedPlayerComponents = UnturnedPlayerComponents.Where(p => p.Assembly != assembly).ToList();
                List<Type> playerComponents = RocketHelper.GetTypesFromParentClass(assembly, typeof(UnturnedPlayerComponent));
                Steam.Players.ForEach(p => Destroy(p.Player.gameObject.GetComponent(playerComponents.GetType())));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void OnEnable()
        {
            try
            {
                U.Events.OnPlayerConnected += addPlayerComponents;
                System.Console.ForegroundColor = ConsoleColor.Cyan;
                List<Type> playerComponents = RocketHelper.GetTypesFromParentClass(assembly, typeof(UnturnedPlayerComponent));
                UnturnedPlayerComponents.AddRange(playerComponents);
                foreach (Type playerComponent in playerComponents)
                {
                    Steam.Players.ForEach(p => p.Player.gameObject.AddComponent(playerComponent.GetType()));
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void addPlayerComponents(IRocketPlayer p)
        {
            foreach (Type component in UnturnedPlayerComponents)
            {
                ((UnturnedPlayer)p).Player.gameObject.AddComponent(component);
            }
        }
    }
}