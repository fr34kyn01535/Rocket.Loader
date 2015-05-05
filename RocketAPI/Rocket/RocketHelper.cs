using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket
{
    internal static class RocketHelper
    {
        public static bool IsUri(string uri) {
            if (String.IsNullOrEmpty(uri)) return false;
            Uri uriOut = null;
            if (Uri.TryCreate(uri, UriKind.Absolute, out uriOut) && (uriOut.Scheme == Uri.UriSchemeHttp || uriOut.Scheme == Uri.UriSchemeHttps))
            {
                return true;
            }
            return false;
        }
    }
}
