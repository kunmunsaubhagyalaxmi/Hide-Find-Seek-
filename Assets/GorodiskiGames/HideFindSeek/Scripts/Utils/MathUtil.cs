using UnityEngine;
using System;

namespace Utilities
{
    public static class MathUtil
    {
        private static readonly System.Random _random;

        static MathUtil()
        {
            _random = new System.Random();
        }

        public static int OneOrMinus()
        {
            return UnityEngine.Random.Range(0, 2) * 2 - 1;
        }

        public static string FloatToString(float value, int decim)
        {
            string _string = "F" + decim;
            return value.ToString(_string);
        }

        public static string NiceCash(int cash)
        {
            string[] suffixes = { "", "k", "m", "b" };
            int suffixIndex;
            int digits;
            if (cash == 0)
            {
                suffixIndex = 0;
                digits = cash.ToString().Length;
            }
            else if (cash > 0)
            {
                suffixIndex = (int)(Mathf.Log10(cash) / 3);
                digits = cash.ToString().Length;
            }
            else
            {
                suffixIndex = (int)(Mathf.Log10(Math.Abs(cash)) / 3);
                digits = Math.Abs(cash).ToString().Length;
            }

            var dividor = Mathf.Pow(10, suffixIndex * 3);
            var text = "";

            if (digits < 4)
                text = (cash / dividor).ToString() + suffixes[suffixIndex];
            else if (digits >= 4 && digits < 7)
                text = (cash / dividor).ToString("F1") + suffixes[suffixIndex];
            else
                text = (cash / dividor).ToString("F2") + suffixes[suffixIndex];
            return text;
        }

        public static long IntToLong(int value)
        {
            return Convert.ToInt64(value);
        }

        public static float GetAngle(Vector3 start, Vector3 end)
        {
            return Mathf.Atan2(start.z - end.z, start.x - end.x) * Mathf.Rad2Deg;
        }

        public static float GetAngle(Vector2 start, Vector2 end)
        {
            return Mathf.Atan2(start.y - end.y, start.x - end.x) * Mathf.Rad2Deg;
        }

        public static long Lerp(double a, double b, float t)
        {
            return (long)(a + (b - a) * Mathf.Clamp01(t));
        }

        public static int Sign(double value)
        {
            return (value >= 0) ? 1 : -1;
        }

        public static int RandomSystem(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        public static float RandomSystem(float min, float max)
        {
            return (float)_random.NextDouble() * (max + .0001f - min) + min;
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max + .0001f);
        }

        public static string IntToHex(uint crc)
        {
            return string.Format("{0:X}", crc);
        }

        public static uint HexToInt(string crc)
        {
            return uint.Parse(crc, System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        public static bool RandomBool
        {
            get
            {
                return UnityEngine.Random.value > 0.5f;
            }
        }

        public static int RandomSign
        {
            get
            {
                return RandomBool ? 1 : -1;
            }
        }

        public static Vector2 RotateAround(Vector2 center, Vector2 point, float angleInRadians)
        {
            angleInRadians *= Mathf.Deg2Rad;
            float cosTheta = Mathf.Cos(angleInRadians);
            float sinTheta = Mathf.Sin(angleInRadians);
            return new Vector2
            {
                x = (cosTheta * (point.x - center.x) - sinTheta * (point.y - center.y)),
                y = (sinTheta * (point.x - center.x) + cosTheta * (point.y - center.y))
            };
        }

        public static string TimeToHMS(float value)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(value);

            string hrsWord = "hr";
            string minWord = "min";
            string secWord = "sec";
            string resultHrs = timeSpan.Hours + hrsWord + " ";
            string resultMin = timeSpan.Minutes + minWord + " ";
            string resultSec = timeSpan.Seconds + secWord;
            string result;

            if (timeSpan.Hours == 0)
            {
                resultHrs = "";
            }

            if (timeSpan.Minutes == 0)
            {
                resultMin = "";
            }

            if (timeSpan.Seconds == 0)
            {
                if (!string.IsNullOrEmpty(resultHrs) || !string.IsNullOrEmpty(resultMin))
                    resultSec = "";
            }
            result = $"{resultHrs}{resultMin}{resultSec}";
            return result;
        }
    }
}