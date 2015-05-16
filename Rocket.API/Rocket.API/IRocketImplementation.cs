using System.Collections.Generic;

namespace Rocket.API
{
    public interface IRocketImplementation
    {
        string InstanceName { get; }
        string HomeFolder { get; }
        string PluginsFolder { get; }
        string LibrariesFolder { get; }
        string LogsFolder { get; }
        string ConfigurationFolder { get; }

        Dictionary<string, string> Translation { get; }

        ImplementationSettingsSection Configuration { get; }
    }
}
