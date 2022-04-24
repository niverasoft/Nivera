using System;
using System.Collections.Generic;

using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace ArkLib.Utils
{
    internal class IntNumberOptions : FieldOptionsNumber<int> { }
    internal class ByteNumberOptions : FieldOptionsNumber<byte> { }
    internal class FloatNumberOptions : FieldOptionsNumber<float> { }

    public static class RandomGen
    {
        private static readonly RandomizerGuid _randomGuid = new RandomizerGuid(new FieldOptionsGuid
        {
            UseNullValues = false,
            Uppercase = true,
            ValueAsString = true
        });

        private static RandomizerBytes _randomBytes = new RandomizerBytes(new FieldOptionsBytes
        {
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerCity _randomCity = new RandomizerCity(new FieldOptionsCity
        {
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerCountry _randomCountry = new RandomizerCountry(new FieldOptionsCountry
        {
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerDateTime _randomDate = new RandomizerDateTime(new FieldOptionsDateTime
        {
            From = DateTime.MinValue,
            To = DateTime.MaxValue,
            Format = "G",
            IncludeTime = true,
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerEmailAddress _randomMail = new RandomizerEmailAddress(new FieldOptionsEmailAddress
        {
            Male = true,
            Female = true,
            Left2Right = false,
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerFirstName _randomName = new RandomizerFirstName(new FieldOptionsFirstName
        {
            Female = true,
            Male = true,
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerFullName _randomFullName = new RandomizerFullName(new FieldOptionsFullName
        {
            Female = true,
            Left2Right = false,
            Male = true,
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerIPv4Address _randomIp4 = new RandomizerIPv4Address(new FieldOptionsIPv4Address
        {
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerIPv6Address _randomIp6 = new RandomizerIPv6Address(new FieldOptionsIPv6Address
        {
            Uppercase = true,
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerIBAN _randomIban = new RandomizerIBAN(new FieldOptionsIBAN
        {
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerLastName _randomLastName = new RandomizerLastName(new FieldOptionsLastName
        {
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerMACAddress _randomMac = new RandomizerMACAddress(new FieldOptionsMACAddress
        {
            Uppercase = true,
            UseNullValues = false,
            ValueAsString = true
        });

        private static RandomizerNumber<int> _randomInt = new RandomizerNumber<int>(new IntNumberOptions
        {
            Max = int.MaxValue,
            Min = int.MinValue,
            UseNullValues = false,
            ValueAsString = true
        });

        private static RandomizerNumber<byte> _randomByte = new RandomizerNumber<byte>(new ByteNumberOptions
        {
            Max = byte.MaxValue,
            Min = byte.MinValue,
            UseNullValues = false,
            ValueAsString = true
        });

        private static RandomizerNumber<float> _randomFloat = new RandomizerNumber<float>(new FloatNumberOptions
        {
            Max = float.MaxValue,
            Min = float.MinValue,
            UseNullValues = false,
            ValueAsString = true
        });

        private static readonly RandomizerStringList _randomStringList = new RandomizerStringList(new FieldOptionsStringList
        {
            UseNullValues = false,
            ValueAsString = true         
        });

        private static RandomizerText _randomText = new RandomizerText(new FieldOptionsText
        {
            UseLetter = true,
            UseLowercase = true,
            UseNullValues = false,
            UseNumber = true,
            UseSpace = false,
            UseSpecial = false,
            UseUppercase = true,
            ValueAsString = true
        });

        public static string RandomGuid()
        {
            return _randomGuid.GenerateAsString();
        }

        public static string RandomString()
        {
            _randomText = new RandomizerText(new FieldOptionsText
            {
                UseLetter = true,
                UseLowercase = true,
                UseNullValues = false,
                UseNumber = true,
                UseSpace = false,
                UseSpecial = false,
                UseUppercase = true,
                ValueAsString = true,
                Max = 15,
                Min = 1
            });

            return _randomText.Generate();
        }

        public static string RandomString(int min, int max)
        {
            _randomText = new RandomizerText(new FieldOptionsText
            {
                UseLetter = true,
                UseLowercase = true,
                UseNullValues = false,
                UseNumber = true,
                UseSpace = false,
                UseSpecial = false,
                UseUppercase = true,
                ValueAsString = true,
                Max = max,
                Min = min
            });

            return _randomText.Generate();
        }

        public static byte[] RandomBytes()
        {
            _randomBytes = new RandomizerBytes(new FieldOptionsBytes
            {
                Max = 20,
                Min = 5,
                UseNullValues = false,
                ValueAsString = true
            });

            return _randomBytes.Generate();
        }

        public static byte[] RandomBytes(int min, int max)
        {
            _randomBytes = new RandomizerBytes(new FieldOptionsBytes
            {
                Max = max,
                Min = min,
                UseNullValues = false,
                ValueAsString = true
            });

            return _randomBytes.Generate();
        }

        public static string RandomBytesString()
        {
            _randomBytes = new RandomizerBytes(new FieldOptionsBytes
            {
                Max = 20,
                Min = 5,
                UseNullValues = false,
                ValueAsString = true
            });

            return _randomBytes.GenerateAsASCIIString();
        }

        public static string RandomBytesString(int min, int max)
        {
            _randomBytes = new RandomizerBytes(new FieldOptionsBytes
            {
                Max = max,
                Min = min,
                UseNullValues = false,
                ValueAsString = true
            });

            return _randomBytes.GenerateAsASCIIString();
        }

        public static byte RandomByte(byte min, byte max)
        {
            _randomByte = new RandomizerNumber<byte>(new ByteNumberOptions
            {
                Max = max,
                Min = min,
                UseNullValues = false,
                ValueAsString = true
            });

            return _randomByte.Generate().GetValueOrDefault();
        }

        public static int RandomInt(int min, int max)
        {
            _randomInt = new RandomizerNumber<int>(new IntNumberOptions
            {
                Max = max,
                Min = min,
                UseNullValues = false,
                ValueAsString = true
            });

            return _randomInt.Generate().GetValueOrDefault();
        }
    }
}
