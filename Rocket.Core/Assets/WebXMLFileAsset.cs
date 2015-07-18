using Rocket.Core.Misc;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Rocket.Core.Assets
{
    public class WebXMLFileAsset<T> : Asset<T> where T : class
    {
        private XmlSerializer serializer;
        private string url;

        public WebXMLFileAsset(string url = null, XmlRootAttribute attr = null)
        {
            serializer = new XmlSerializer(typeof(T), attr);
            this.url = url;
            Load();
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
                if (!String.IsNullOrEmpty(url))
                {
                    RocketWebClient webclient = new RocketWebClient();
                    webclient.DownloadStringCompleted += (object sender, System.Net.DownloadStringCompletedEventArgs e) =>
                    {
                        using (StringReader reader = new StringReader(e.Result))
                        {
                            instance = (T)serializer.Deserialize(reader);
                            callback(this);
                        }
                    };
                }else
                {
                    throw new ArgumentNullException("WebXMLFileAsset url is blank");
                }
                if (callback != null)
                    callback(this);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to deserialize WebXMLFileAsset: {0}", url), ex);
            }
        }
    }
}