using SDG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rocket
{
    public class RocketPluginManager : MonoBehaviour
    {
        public List<Assembly> Assemblies;

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            #region Handling additional assemblies
            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs args)
            {   
                string file;
                if (additionalLibraries.TryGetValue(args.Name, out file))
                {
                    return Assembly.Load(File.ReadAllBytes(file));
                }
                return null;
            };
            loadLibraries();
            #endregion

            Assemblies = loadAssemblies();

            Logger.Log("Loading Rocket components...");
            List<RocketComponent> rocketComponents = getTypes<RocketComponent>(Assemblies,typeof(RocketComponent));


            foreach (RocketComponent component in rocketComponents)
            {
                RocketAPI.Instance.gameObject.AddComponent(component.GetType());
            }

            Logger.Log("Loading commands...");
            List<Command> commands = getTypes<Command>(Assemblies, typeof(Command));
            List<Command> rocketCommands = getTypes<Command>(new List<Assembly>() { Assembly.GetExecutingAssembly() }, typeof(Command));
            commands.AddRange(rocketCommands);

            foreach (Command command in commands)
            {
                Commands.RegisterCommand(command);
            }

            SDG.Steam.serverConnected += clientConnected;
        }

        private void clientConnected(Steamworks.CSteamID cSteamID)
        {
            List<RocketPlayerComponent> rocketPlayerComponent = getTypes<RocketPlayerComponent>(Assemblies, typeof(RocketPlayerComponent));
            GameObject gameobject = PlayerTool.getPlayer(cSteamID).transform.gameObject;
            foreach (RocketPlayerComponent component in rocketPlayerComponent)
            {
                gameobject.AddComponent(component.GetType());
            }
        }

        #region Handling additional assemblies
        private Dictionary<string, string> additionalLibraries = new Dictionary<string, string>();
        private void loadLibraries()
        {
            FileInfo[] libraries = new DirectoryInfo(RocketAPI.HomeFolder + "Plugins/Libraries/").GetFiles("*.dll");
            foreach (FileInfo library in libraries)
            {
                try
                {
                    AssemblyName name = AssemblyName.GetAssemblyName(library.FullName);
                    additionalLibraries.Add(name.FullName, library.FullName);
                }
                catch { } 
            }
        }
        #endregion

        private List<Assembly> loadAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();
            try
            {    
                FileInfo[] pluginsLibraries = new DirectoryInfo(RocketAPI.HomeFolder + "Plugins/").GetFiles("*.dll");

                foreach (FileInfo library in pluginsLibraries)
                {
                    Assembly assembly = Assembly.Load(File.ReadAllBytes(library.FullName));
                    assemblies.Add(assembly);

                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return assemblies;
        }

        public List<Type> getTypes(List<Assembly> assemblies)
        {
            List<Type> allTypes = new List<Type>();
            foreach (Assembly assembly in assemblies) {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types;
                }
                allTypes.AddRange(types);
            }
            return allTypes;
        }
        public List<T> getTypes<T>(List<Assembly> assemblies,Type parentClass)
        {
            List<T> allTypes = new List<T>();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types;
                }
                foreach (Type type in types) {
                    if (type.IsSubclassOf(parentClass)) {
                        allTypes.Add((T)Activator.CreateInstance(type));
                    }
                }
            }
            return allTypes;
        }
    }
}
