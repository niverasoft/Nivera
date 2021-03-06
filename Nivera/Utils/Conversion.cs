using System;

using UnityEngine;

namespace Nivera.Utils
{
    public static class Conversion
    {
        public static float[] ToFloatArray(byte[] array)
        {
            float[] floatArr = new float[array.Length / 4];

            for (int i = 0; i < floatArr.Length; i++)
            {
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(array, i * 4, 4);

                floatArr[i] = BitConverter.ToSingle(array, i * 4) / 0x80000000;
            }

            return floatArr;
        }

        public static AudioClip ToAudioClip(string name, float[] floatArray)
        {
            var clip = AudioClip.Create(name, floatArray.Length, 1, 44100, false);

            clip.SetData(floatArray, 0);

            return clip;
        }
    }
}
