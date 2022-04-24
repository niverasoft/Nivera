using System;

namespace ArkLib.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class DescriptionAttribute : Attribute
    {
        private string _description;

        public DescriptionAttribute(object description)
            => _description = description?.ToString();

        internal string GetDescription()
        {
            return _description;
        }
    }
}