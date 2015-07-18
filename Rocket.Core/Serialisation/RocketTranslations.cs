using Rocket.API.Collections;

namespace Rocket.Core.Serialization
{
    public class RocketTranslations : TranslationList
    {
        TranslationList translations = new TranslationList()
        {
            {"rocket_join_public","{0} connected to the server" },
            {"rocket_leave_public","{0} disconnected from the server"},
            {"rocket_restart_warning_public","This server will be restarted in 30 seconds"}
        };
    }
}
