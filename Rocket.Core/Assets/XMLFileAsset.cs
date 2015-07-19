using Rocket.Core.Assets;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Rocket.Core.Assets
{
    public class XMLFileAsset<T> : Asset<T> where T : class
    {
        private XmlSerializer serializer;
        private string file;

        public XMLFileAsset(string file, XmlRootAttribute attr = null)
        {
            if (attr != null)
                serializer = new XmlSerializer(typeof(T), attr);
            else
                serializer = new XmlSerializer(typeof(T));
            this.file = file;
            Load();
        }

        public override T Save(T instance = null)
        {
            try
            {
                string directory = Path.GetDirectoryName(file);
                if (!String.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);
                using (StreamWriter writer = new StreamWriter(file))
                {
                    if(instance == null)
                    {
                        instance = Activator.CreateInstance<T>();
                    }
                    this.instance = instance;
                    serializer.Serialize(writer,instance);
                    return instance;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to serialize configuration file: {0}", file), ex);
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
                else
                {
                    Save();
                }
                if(callback != null)
                    callback(this);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to deserialize configuration file: {0}", file), ex);
            }
        }
        
    }
}
