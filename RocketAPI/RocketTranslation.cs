﻿using Rocket.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace Rocket.RocketAPI
{
    internal static class RocketTranslation
    {
        private static string translationFile = "{0}/Rocket.{1}.translation.xml";
       
        private static Dictionary<string, string> defaultTranslations = new Dictionary<string, string>()
        {
            {"command_generic_invalid_parameter","Invalid Parameter"},
            {"command_generic_teleport_while_driving_error","You cannot teleport while driving or riding in a vehicle."},
            {"command_god_enable_console","{0} enabled Godmode"},
            {"command_god_enable_private","You can feel the strength now..."},
            {"command_god_disable_console","{0} disabled Godmode"},
            {"command_god_disable_private","The godly powers left you..."},
            {"command_bed_no_bed_found_private","You do not have a bed to teleport to."},
            {"command_i_giving_console","Giving {0} item {1}:{2}"},
            {"command_i_giving_private","Giving you item {0}x {1} ({2})"},
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
            {"command_tp_teleport_private","Teleported to {1}"},
            {"command_tp_failed_find_destination","Failed to find destination"},
            {"command_tphere_teleport_console","{0} was teleported to {1}"},
            {"command_tphere_teleport_from_private","Teleported {0} to you"},
            {"command_tphere_teleport_to_private","You were teleported to {0}"},
            {"command_tphere_failed_find_player","Failed to find player"},
            
            {"rocket_join_public","{0} connected to the server"},
            {"rocket_leave_public","{0} disconnected from the server"},

        };

        private static Dictionary<string, string> translations = null;

        public static string Translate(string translationKey, params object[] placeholder)
        {
            string value = translationKey;
            if (translations != null)
            {
                translations.TryGetValue(translationKey, out value);
                if (value.Contains("{0}") && placeholder != null && placeholder.Length != 0)
                {
                    value = String.Format(value, placeholder);
                }
            }
            return value;
        }

        internal static void LoadTranslations()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Rocket.RocketAPI.RocketTranslationHelper.Translation[]), new XmlRootAttribute() { ElementName = "Translations" });
            string rocketTranslation = String.Format(translationFile, RocketSettings.HomeFolder, RocketSettings.LanguageCode);

            if (File.Exists(rocketTranslation))
            {
                using (StreamReader r = new StreamReader(rocketTranslation))
                {
                    translations = ((Rocket.RocketAPI.RocketTranslationHelper.Translation[])serializer.Deserialize(r)).ToDictionary(i => i.Id, i => i.Value);
                }
                foreach (string key in defaultTranslations.Keys)
                {
                    if (!translations.ContainsKey(key)) {
                        translations.Add(key, defaultTranslations[key]);
                    }
                }
            }
            else
            {
                if (RocketSettings.LanguageCode != "en")
                {
                    rocketTranslation = String.Format(translationFile, RocketSettings.HomeFolder, "en");
                    Logger.LogWarning(Path.GetFileName(rocketTranslation) + " could not be found, recovering default language");
                }
                translations = defaultTranslations;
            }

            using (StreamWriter w = new StreamWriter(rocketTranslation))
            {
                serializer.Serialize(w, translations.Select(kv => new Rocket.RocketAPI.RocketTranslationHelper.Translation() { Id = kv.Key, Value = kv.Value }).ToArray());
            }
        }
    }
}
