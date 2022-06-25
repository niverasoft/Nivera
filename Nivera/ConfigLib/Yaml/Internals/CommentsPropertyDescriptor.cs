using System;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Nivera.ConfigLib.Yaml.Internals
{
    public sealed class CommentsPropertyDescriptor : IPropertyDescriptor
    {
        private readonly IPropertyDescriptor baseDescriptor;

        public CommentsPropertyDescriptor(IPropertyDescriptor baseDescriptor)
        {
            this.baseDescriptor = baseDescriptor;
            Name = baseDescriptor.Name;
        }

        public string Name { get; set; }
        public Type Type => baseDescriptor.Type;
        public Type TypeOverride
        {
            get => baseDescriptor.TypeOverride;
            set => baseDescriptor.TypeOverride = value;
        }
        public int Order { get; set; }
        public ScalarStyle ScalarStyle
        {
            get => baseDescriptor.ScalarStyle;
            set => baseDescriptor.ScalarStyle = value;
        }
        public bool CanWrite => baseDescriptor.CanWrite;

        public void Write(object target, object value)
        {
            baseDescriptor.Write(target, value);
        }

        public T GetCustomAttribute<T>()
            where T : Attribute
        {
            return baseDescriptor.GetCustomAttribute<T>();
        }

        public IObjectDescriptor Read(object target)
        {
            string desc = GetDescriptionWorkaround(target);
            return desc != null
                ? new CommentsObjectDescriptor(baseDescriptor.Read(target), desc)
                : baseDescriptor.Read(target);
        }

        public string GetDescriptionWorkaround(object target)
        {
            object[] attributes = target.GetType().GetCustomAttributes(true);
            object attribute = null;

            foreach (var attr in attributes)
            {
                if (attr.GetType().FullName == "Nivera.Attributes.DescriptionAttribute")
                {
                    attribute = attr;
                    break;
                }
            }

            if (attribute == null)
                return null;

            return attribute.GetType().GetMethod("GetDescription").Invoke(attribute, null).ToString();
        }
    }
}