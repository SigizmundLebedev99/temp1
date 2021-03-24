using System;

namespace MonoGame.Squid.Util
{
    /// <summary>
    /// Helper class to represent a font.
    /// This class will eventually be obsolete. Do not use.
    /// </summary>
    public class Font
    {
        public static readonly string Default = "default";

        public string Name { get; set; }
        
        public bool Bold;
        public bool Italic;
        public bool Underlined;
        public bool International;

        public int Size;
    }
}
