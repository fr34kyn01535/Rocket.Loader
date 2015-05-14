using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Rocket.API
{
    public interface IRocketConfiguration
    {
        IRocketConfiguration DefaultConfiguration { get; }
    }
}