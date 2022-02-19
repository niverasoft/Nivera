using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RandomDataGenerator;
using RandomDataGenerator.Data;
using RandomDataGenerator.Enums;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace AtlasLib.Utils
{
    public static class RandomGen
    {
        private static readonly FieldOptionsGuid _options = new FieldOptionsGuid
        {
            UseNullValues = false,
        };

        private static readonly RandomizerGuid _randomGuid = new RandomizerGuid(_options);

        public static string NextString()
        {
            return _randomGuid.GenerateAsString();
        }
    }
}
