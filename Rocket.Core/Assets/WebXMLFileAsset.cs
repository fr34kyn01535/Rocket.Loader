using Rocket.Core.Utils;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Rocket.Core.Assets
{
    public class WebXMLFileAsset<T> : Asset<T> where T : class
    {
        private XmlSerializer serializer;
        private string url;

        public WebXMLFileAsset(string url = null, XmlRootAttribute attr = null, AssetLoaded<T> callback = null)
        {
            serializer = new XmlSerializer(typeof(T), attr);
            this.url = url;
            Load(callback);
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
                        RocketDispatcher.QueueOnMainThread(() =>
                        {
                            using (StringReader reader = new StringReader(e.Result))
                            {
                                instance = (T)serializer.Deserialize(reader);

                                if (callback != null)
                                    callback(this);
                            }
                        });
                    };
                    webclient.DownloadStringAsync(new Uri(url));
                }else
                {
                    throw new ArgumentNullException("WebXMLFileAsset url is blank");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to deserialize WebXMLFileAsset: {0}", url), ex);
            }
        }
    }
}