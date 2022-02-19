namespace AtlasLib.Logging
{
    public struct BasicLogTag : ILogTag
    {
        public string Tag { get; set; }

        public bool UpperCase { get; set; }
        public bool IsPriority { get; set; }
        public bool IsFatal { get; set; }

        public LogColor Color { get; set; }

        public void ProcessTag(ref string message)
        {
            message = $"[{Color}:{(UpperCase ? Tag.ToUpper() : Tag)} / {Channel()}] {message}";
        }

        private int Channel()
        {
            if (IsPriority)
                return 10;

            if (IsFatal)
                return 15;

            if (UpperCase)
                return 5;

            return 1;
        }

        public static readonly ILogTag Information = new BasicLogTag { Tag = "Information", UpperCase = false, IsPriority = false, IsFatal = false, Color = LogColor.White };
        public static readonly ILogTag Warning = new BasicLogTag { Tag = "Warning", UpperCase = false, IsPriority = true, IsFatal = false, Color = LogColor.Yellow };
        public static readonly ILogTag Debug = new BasicLogTag { Tag = "Debug", UpperCase = false, IsPriority = false, IsFatal = false, Color = LogColor.Blue };
        public static readonly ILogTag Verbose = new BasicLogTag { Tag = "Verbose", UpperCase = false, IsPriority = false, IsFatal = false, Color = LogColor.Cyan };
        public static readonly ILogTag Error = new BasicLogTag { Tag = "Error", UpperCase = false, IsPriority = true, IsFatal = false, Color = LogColor.Red };
        public static readonly ILogTag Fatal = new BasicLogTag { Tag = "Fatal", UpperCase = false, IsPriority = true, IsFatal = true, Color = LogColor.DarkRed };
    }
}
