// *********************************************************************************
// The MIT License (MIT)
// Copyright (c) 2020 SpiralBlack https://github.com/SpiralBlack15
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// *********************************************************************************

using System;
using UnityEngine;

namespace Spiral.Core
{
    public static class MathTools
    {
        public static string MantissString(this float value, int mantiss)
        {
            return (mantiss < 0) ? value.ToString() : value.ToString($"F{mantiss}");
        }

        public static float Mantissed(this float value, int mantiss)
        {
            return Convert.ToSingle(Math.Round(value, mantiss));
        }

        public static float SafeRead(this float read, float safeValue)
        {
            if (float.IsNaN(read))      return safeValue;
            if (float.IsInfinity(read)) return safeValue;
            return read;
        }

        public static float ClampLow(this float value, float low)
        {
            return (value < low) ? low : value;
        }

        public static int ClampLow(this int value, int low)
        {
            return (value < low) ? low : value;
        }

        public static float ClampHigh(this float value, float high)
        {
            return (value > high) ? high : value;
        }

        public static int ClampHigh(this int value, int high)
        {
            return (value > high) ? high : value;
        }

        public static float Clamp0P(this float value)
        {
            return value.ClampLow(0);
        }

        public static int Clamp0P(this int value)
        {
            return value.ClampLow(0);
        }

        public static float Clamp0N(this float value)
        {
            return value.ClampHigh(0);
        }

        public static int Clamp0N(this int value)
        {
            return value.ClampHigh(0);
        }

        public static float Clamp(this float value, float low, float high)
        {
            return Mathf.Clamp(value, low, high);
        }

        public static int Clamp(this int value, int low, int hight)
        {
            return Mathf.Clamp(value, low, hight);
        }

        public static bool Between(this float value, float min, float max, bool include = true)
        {
            if (min > max)
            {
                float swap = max;
                max = min;
                min = swap;
            }

            if (include) return (value >= min) && (value <= max);
            else return (value > min) && (value < max);
        }

        public static bool Between(this int value, int min, int max, bool include = true)
        {
            if (include) return (value >= min) && (value <= max);
            else return (value > min) && (value < max);
        }

        public static int Round(this float value)
        {
            return Mathf.RoundToInt(value);
        }

        public static int Floor(this float value)
        {
            return Mathf.FloorToInt(value);
        }

        public static int Ceil(this float value)
        {
            return Mathf.CeilToInt(value);
        }

        public static float Abs(this float value)
        {
            return Mathf.Abs(value);
        }

        public static int Abs(this int value)
        {
            return Mathf.Abs(value);
        }

        public static int ToInt32(this string str, bool safety = false)
        {
            int result;
            if (safety)
            {
                try
                {
                    result = Convert.ToInt32(str);
                }
                catch
                {
                    Debug.Log("Error while converting Str to Int32");
                    result = 0;
                }
            }
            else
            {
                result = Convert.ToInt32(str);
            }
            return result;
        }

        public static float ToFloat(this string str, bool safety = false)
        {
            float result;
            if (safety)
            {
                try
                {
                    result = Convert.ToSingle(str);
                }
                catch
                {
                    Debug.Log("Error while converting Str to Single");
                    result = 0;
                }
            }
            else
            {
                result = Convert.ToSingle(str);
            }
            return result;
        }
        
        public static Vector2 ToVector2(this string str, char split = 'x', bool safety = false)
        {
            string[] values = str.Split(split);
            Vector2 answer = new Vector2();
            try
            {
                answer.x = values[0].ToFloat(safety);
                answer.y = values[1].ToFloat(safety);
            }
            catch
            {
                Debug.Log("Error while converting Str to Vector2");
            }
            return answer;
        }

        public static Vector2Int ToVector2Int(this string str, char split = 'x', bool safety = false)
        {
            string[] values = str.Split(split);
            Vector2Int answer = new Vector2Int();
            try
            {
                answer.x = values[0].ToInt32(safety);
                answer.y = values[1].ToInt32(safety);
            }
            catch
            {
                Debug.Log("Error while converting Str to Vector2");
            }
            return answer;
        }
    }
}
