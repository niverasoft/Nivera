namespace AtlasLib.Logging
{
    public static class LogExtensions
    {
        public static string WithTag(this string str, ILogTag tag)
        {
            tag?.ProcessTag(ref str);

            return str;
        }
    }
}
