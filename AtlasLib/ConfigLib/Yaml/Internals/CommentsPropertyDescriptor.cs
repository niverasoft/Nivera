﻿using System;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AtlasLib.ConfigLib.Yaml.Internals
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
            AtlasLib.Attributes.DescriptionAttribute description = baseDescriptor.GetCustomAttribute<AtlasLib.Attributes.DescriptionAttribute>();
            return description != null
                ? new CommentsObjectDescriptor(baseDescriptor.Read(target), description.GetDescription())
                : baseDescriptor.Read(target);
        }
    }
}
