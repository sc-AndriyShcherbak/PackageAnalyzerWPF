using PackageAnalyzer.Core.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PackageAnalyzerDesktop.Core
{
    internal static class PackageAnalyzerAdapter
    {
        public static string GetSitecoreRoles(string filePath)
        {
            WebConfigInfoReader webConfigInfoReader = new WebConfigInfoReader();

            string result = string.Empty;
            string pattern = @"[\/\\[\]]";

            foreach (var item in webConfigInfoReader.ReadRole(filePath))
            {
                string key = Regex.Replace(item.Key, pattern, "").Replace("configs", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("web.config", "", StringComparison.OrdinalIgnoreCase);
                key = ExtractLastWord(key);
                result += key + " = " + item.Value + Environment.NewLine;
            }

            return result;
        }

        static string ExtractLastWord(string inputString)
        {
            Match match = Regex.Match(inputString, @"[^-]*$");
            return match.Success ? match.Value : null;
        }

        public static string GetSitecoreVersions(string filePath)
        {
            SitecoreVersionReader sitecoreVersionReader = new SitecoreVersionReader();

            string result = string.Empty;
            string pattern = @"[\/\\[\]]";

            var versions = sitecoreVersionReader.Read(filePath);

            if (versions == null || versions.Count() == 0)
            {
                return result;
            }

            if (versions.Values.Distinct().Count() == 1)
            {
                return versions.Values.FirstOrDefault();
            }
            else
            {
                foreach (var item in sitecoreVersionReader.Read(filePath))
                {
                    //string key = Regex.Replace(item.Key, pattern, "").Replace("configs", "", StringComparison.OrdinalIgnoreCase)
                    //    .Replace("web.config", "", StringComparison.OrdinalIgnoreCase);
                    result += item.Key + " = " + item.Value + Environment.NewLine;
                }
            }
            return result;
        }

    }
}
