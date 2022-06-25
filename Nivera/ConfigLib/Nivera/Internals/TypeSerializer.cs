using System;
using System.Reflection;
using System.Linq;
using System.Text;

using Nivera.Attributes;

namespace Nivera.ConfigLib.Nivera.Internals
{
    public class TypeSerializer
    {
        public const string CommentStart = ">>>";
        public const string GenerationDelimiterStart = "<=================================";
        public const string GenerationDelimiterEnd = "=================================>";

        private Type _type;
        private object _typeInstanceRef;

        public TypeSerializer(Type type, object typeInstanceRef)
        {
            _type = type;
            _typeInstanceRef = typeInstanceRef;
        }

        public string Serialize()
        {
            StringBuilder stringBuilder = new StringBuilder();

            AppendDate(stringBuilder);

            int index = 0;

            foreach (FieldInfo field in _type.GetFields())
            {
                if (!field.IsPublic)
                    continue;

                ConfigWriter writer = new ConfigWriter();

                new FieldSerializer(field.Name, field, _typeInstanceRef).Serialize(writer);

                index++;

                AppendToBuilder(index == 0, field.GetCustomAttributes<DescriptionAttribute>()?.ToArray() ?? null, writer, stringBuilder);
            }

            foreach (PropertyInfo prop in _type.GetProperties())
            {
                if (prop.GetMethod == null || prop.SetMethod == null || !prop.GetMethod.IsPublic || !prop.SetMethod.IsPublic)
                    continue;

                ConfigWriter writer = new ConfigWriter();

                new PropertySerializer(prop.Name, prop, _typeInstanceRef).Serialize(writer);

                index++;

                AppendToBuilder(index == 0, prop.GetCustomAttributes<DescriptionAttribute>()?.ToArray() ?? null, writer, stringBuilder);
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            AppendDate(stringBuilder);

            return stringBuilder.ToString();
        }

        private void AppendDate(StringBuilder stringBuilder)
        {
            stringBuilder.Append($"{GenerationDelimiterStart} CONFIG FILE GENERATED AT {DateTime.Now.ToLocalTime().ToString("G")} {GenerationDelimiterEnd}");
        }

        private void AppendToBuilder(bool isStart, DescriptionAttribute[] desc, ConfigWriter writer, StringBuilder stringBuilder)
        {
            if (!isStart)
                stringBuilder.AppendLine();

            if (desc != null && desc.Length > 0)
            {
                foreach (DescriptionAttribute descriptionAttribute in desc)
                {
                    if (string.IsNullOrWhiteSpace(descriptionAttribute.GetDescription()))
                        continue;

                    stringBuilder.AppendLine($"{CommentStart} {descriptionAttribute.GetDescription()}");
                }
            }

            foreach (string str in writer.ReadLines())
            {
                stringBuilder.AppendLine(str);
            }
        }
    }
}
