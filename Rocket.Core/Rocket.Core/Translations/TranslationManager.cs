using Rocket.Core.Logging;
using Rocket.RocketAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;


namespace Rocket.Core
{
    public class TranslationManager : MonoBehaviour
    {
        public class TranslationEntry
        {
            [XmlAttribute]
            public string Id;
            [XmlAttribute]
            public string Value;
        }
        internal void Awake() {
            ReloadTranslations();
        }

        private static string pluginTranslationFile = "{0}Plugins/{1}/{1}.{2}.translation.xml";
        private static string pluginFolder = "{0}Plugins/{1}/";

        private static string translationFile = "{0}/Rocket.{1}.translation.xml";
        #region defaultTranslations
        private static Dictionary<string, string> defaultTranslations = new Dictionary<string, string>()
        { 
            {"command_generic_failed_find_player","Failed to find player"},
            {"command_generic_invalid_parameter","Invalid Parameter"},
            {"command_generic_target_player_not_found","Target player not found"},
            {"command_generic_teleport_while_driving_error","You cannot teleport while driving or riding in a vehicle."},
            {"command_god_enable_console","{0} enabled Godmode"},
            {"command_god_enable_private","You can feel the strength now..."},
            {"command_god_disable_console","{0} disabled Godmode"},
            {"command_god_disable_private","The godly powers left you..."},
            {"command_vanish_enable_console","{0} enabled Vanishmode"},
            {"command_vanish_enable_private","You are vanished now..."},
            {"command_vanish_disable_console","{0} disabled Vanishmode"},
            {"command_vanish_disable_private","You are no longer vanished..."},

            {"command_duty_enable_console","{0} is in duty"},
            {"command_duty_enable_private","You are in duty now..."},
            {"command_duty_disable_console","{0} is no longer in duty"},
            {"command_duty_disable_private","You are no longer in duty..."},

            {"command_bed_no_bed_found_private","You do not have a bed to teleport to."},
            {"command_i_giving_console","Giving {0} item {1}:{2}"},
            {"command_i_giving_private","Giving you item {0}x {1} ({2})"},
            {"command_z_giving_console","Spawning {1} zombies near {0}"},
            {"command_z_giving_private","Spawning {0} zombies nearby"},
            {"command_i_giving_failed_private","Failed giving you item {0}x {1} ({2})"},
            {"command_v_giving_console","Giving {0} vehicle {1}"},
            {"command_v_giving_private","Giving you a {0} ({1})"},
            {"command_v_giving_failed_private","Failed giving you a {0} ({1})"},
            {"command_tps_tps","TPS: {0}"},
            {"command_tps_running_since","Running since: {0} UTC"},
            {"command_p_reload_private","Reloaded permissions"},
            {"command_p_groups_private","{0} groups are: {1}"},
            {"command_p_permissions_private","{0} permissions are: {1}"},
            {"command_tp_teleport_console","{0} teleported to {1}"},
            {"command_tp_teleport_private","Teleported to {0}"},
            {"command_tp_failed_find_destination","Failed to find destination"},
            {"command_tphere_teleport_console","{0} was teleported to {1}"},
            {"command_tphere_teleport_from_private","Teleported {0} to you"},
            {"command_tphere_teleport_to_private","You were teleported to {0}"},
            {"command_clear_error","There was an error clearing {0} inventory."},
            {"command_clear_private","Your inventory was cleared!"},
            {"command_clear_other","Your inventory was cleared by {0}!"},
            {"command_clear_other_success","You successfully cleared {0} inventory."},
            {"command_investigate_private","{0} SteamID64 is {1}"},
            {"command_heal_success_me","{0} was successfully healed"},
            {"command_heal_success_other","You were healed by {0}"},
            {"command_heal_success","You were healed"},
            {"command_compass_facing_private","You are facing {0}"},
            {"command_compass_north","N"},
            {"command_compass_east","E"},
            {"command_compass_south","S"},
            {"command_compass_west","W"},
            {"command_compass_northwest","NW"},
            {"command_compass_northeast","NE"},
            {"command_compass_southwest","SW"},
            {"command_compass_southeast","SE"},
            {"rocket_join_public","{0} connected to the server"},
            {"rocket_leave_public","{0} disconnected from the server"},
            {"rocket_restart_warning_public","This server will be restarted in 30 seconds"},

            {"command_rocket_plugins_loaded","Plugins loaded: {0}"},
            {"command_rocket_plugins_unloaded","Plugins unloaded: {0}"},
            {"command_rocket_reload_plugin","Reloading {0}"},
            {"command_rocket_not_loaded","The plugin {0} is not loaded"},
            {"command_rocket_unload_plugin","Unloading {0}"},
            {"command_rocket_load_plugin","Loading {0}"},
            {"command_rocket_already_loaded","The plugin {0} is already loaded"},
            {"command_rocket_reload","Reloading Rocket"},
            {"command_rocket_plugin_not_found","Plugin {0} not found"},
            
        };
        #endregion
        private static Dictionary<string, string> translations = null;

        public static string Translate(string translationKey, params object[] placeholder)
        {
            try
            {
                string value = null;
                if (translations != null)
                {
                    translations.TryGetValue(translationKey, out value);
                    if (value == null) value = translationKey;

                    for (int i = 0; i < placeholder.Length; i++)
                    {
                        if (placeholder[i] == null) placeholder[i] = "NULL";
                    }

                    if (value.Contains("{0}") && placeholder != null && placeholder.Length != 0)
                    {
                        value = String.Format(value, placeholder);
                    }
                }
                return value;
            }
            catch (Exception er)
            {
                Logger.LogError("Error fetching translation for " + translationKey+": "+er.ToString());
                return translationKey;
            }
        }

        internal static void ReloadTranslations()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TranslationEntry[]), new XmlRootAttribute() { ElementName = "Translations" });
                string rocketTranslation = String.Format(translationFile, Rocket.HomeFolder, SettingsManager.LanguageCode);

                if (File.Exists(rocketTranslation))
                {
                    using (StreamReader r = new StreamReader(rocketTranslation))
                    {
                        translations = ((TranslationEntry[])serializer.Deserialize(r)).ToDictionary(i => i.Id, i => i.Value);
                    }
                    foreach (string key in defaultTranslations.Keys)
                    {
                        if (!translations.ContainsKey(key))
                        {
                            translations.Add(key, defaultTranslations[key]);
                        }
                    }
                }
                else
                {
                    if (SettingsManager.LanguageCode != "en")
                    {
                        Logger.LogWarning(Path.GetFileName(rocketTranslation) + " could not be found, recovering default language");
                        rocketTranslation = String.Format(translationFile, Rocket.HomeFolder, "en");
                    }
                    translations = defaultTranslations;
                }

                using (StreamWriter w = new StreamWriter(rocketTranslation))
                {
                    serializer.Serialize(w, translations.Select(kv => new TranslationEntry() { Id = kv.Key, Value = kv.Value }).ToArray());
                }

            }
            catch (Exception ex)
            {
                Logger.LogError("Error loading translations: "+ex.ToString());
            }
        }

        internal static Dictionary<string, string> LoadTranslation(string assemblyName, Dictionary<string, string> fallback)
        {
            if (!Directory.Exists(String.Format(pluginFolder, Rocket.HomeFolder, assemblyName))) return fallback;
            XmlSerializer serializer = new XmlSerializer(typeof(TranslationEntry[]), new XmlRootAttribute() { ElementName = "Translations" });
            Dictionary<string, string> translations;
            string rocketTranslation = String.Format(pluginTranslationFile, Rocket.HomeFolder, assemblyName, SettingsManager.LanguageCode);

            if (File.Exists(rocketTranslation))
            {
                using (StreamReader r = new StreamReader(rocketTranslation))
                {
                    translations = ((TranslationEntry[])serializer.Deserialize(r)).ToDictionary(i => i.Id, i => i.Value);
                }
                foreach (string key in fallback.Keys)
                {
                    if (!translations.ContainsKey(key))
                    {
                        translations.Add(key, fallback[key]);
                    }
                }
            }
            else
            {
                if (SettingsManager.LanguageCode != "en")
                {
                    rocketTranslation = String.Format(pluginTranslationFile, Rocket.HomeFolder, assemblyName, "en");
                    Logger.LogWarning(Path.GetFileName(rocketTranslation) + " could not be found, recovering default language");
                }
                translations = fallback;
            }
            if (translations.Count != 0)
            {
                using (StreamWriter w = new StreamWriter(rocketTranslation))
                {
                    serializer.Serialize(w, translations.Select(kv => new TranslationEntry() { Id = kv.Key, Value = kv.Value }).ToArray());
                }
            }
            return translations;
        }
    }
}
