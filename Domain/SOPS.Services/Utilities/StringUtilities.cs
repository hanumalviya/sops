using System;
using System.Linq;

namespace SOPS.Services.Utilities
{
    public static class StringUtilities
    {
        public static bool InsensitiveContains(this string text, string phrase)
        {
            return text != null && (phrase == null || phrase == string.Empty || text.ToLower().Contains(phrase.ToLower()));
        }
    }
}
