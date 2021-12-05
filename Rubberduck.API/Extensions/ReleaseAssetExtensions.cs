using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace Rubberduck.API.Extensions
{
    internal static class ReleaseAssetExtensions
    {
        public static bool IsInstallerAsset(this ReleaseAsset asset) => asset.Name.StartsWith("Rubberduck.Setup") && asset.Name.EndsWith(".exe");
        public static bool IsXmlDocAsset(this ReleaseAsset asset) => asset.Name.EndsWith(".xml");
    }
}
