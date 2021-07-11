using System;
using System.IO;

namespace GlobalValues
{
    public static class G
    {
        public static string DS { get; } = Path.DirectorySeparatorChar.ToString();
        public static string AssetPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"{DS}10KRankerAssets";
    }
}
