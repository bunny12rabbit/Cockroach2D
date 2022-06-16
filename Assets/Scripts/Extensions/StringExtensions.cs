using UnityEngine;

namespace Extensions
{
    public static class StringExtensions
    {
        public static string Colorize(this string str, string color)
        {
            return $"<color={color}>{str}</color>";
        }

        public static string Colorize(this string str, Color color)
        {
            return str.Colorize(color.ToHexString());
        }

        public static string ToHexString(this Color color, bool addAlpha = false)
        {
            byte ComponentToByte(float colorComponent)
            {
                return (byte) (colorComponent * 255);
            }

            var str = $"#{ComponentToByte(color.r):X2}{ComponentToByte(color.g):X2}{ComponentToByte(color.b):X2}";

            if (addAlpha)
                str += ComponentToByte(color.a).ToString("X2");

            return str;
        }
    }
}