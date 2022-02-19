namespace AtlasLib.Logging
{
    public interface ILogTag
    {
        string Tag { get; set; }

        bool IsPriority { get; set; }
        bool IsFatal { get; set; }
        bool UpperCase { get; set; }

        LogColor Color { get; set; }

        void ProcessTag(ref string message);
    }
}
