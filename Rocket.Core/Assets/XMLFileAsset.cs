﻿using Rocket.Core.Assets;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Rocket.Core.Assets
{
    public class XMLFileAsset<T> : Asset<T> where T : class, IDefaultable
    {
        private XmlSerializer serializer;
        private string file;
        T defaultInstance;

        public XMLFileAsset(string file, Type[] extraTypes = null, T defaultInstance = null)
        {
            serializer = new XmlSerializer(typeof(T), extraTypes);
            this.file = file;
            this.defaultInstance = defaultInstance;
            Load(null, true);
        }

        public override T Save()
        {
            try
            {
                string directory = Path.GetDirectoryName(file);
                if (!String.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);
                using (StreamWriter writer = new StreamWriter(file))
                {
                    if (instance == null)
                    {
                        if (defaultInstance == null)
                        {
                            instance = Activator.CreateInstance<T>();
                            instance.LoadDefaults();
                        }
                        else
                        {
                            instance = defaultInstance;
                        }
                    }
                    serializer.Serialize(writer,instance);
                    return instance;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to serialize XMLFileAsset: {0}", file), ex);
            }
        }

        public override void Load(AssetLoaded<T> callback = null, bool update = false)
        {
            try
            {
                if (instance != null && !update)
                {
                    if (callback != null) { callback(this); }
                    return;
                }
                if (!String.IsNullOrEmpty(file) && File.Exists(file))
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        instance = (T)serializer.Deserialize(reader);
                    }
                }

                Save();

                if (callback != null)
                    callback(this);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to deserialize XMLFileAsset: {0}", file), ex);
            }
        }
        
    }
}
