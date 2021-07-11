using System;
using System.IO;

namespace GlobalValues
{
    public static class G
    {
        public static string AssetPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static string DS { get; } = Path.DirectorySeparatorChar.ToString();
    }
}
