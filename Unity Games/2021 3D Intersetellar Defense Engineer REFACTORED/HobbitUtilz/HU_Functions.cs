using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HobbitUtilz
{
    /// <summary>
    /// Class that has commonly used stand alone functions.
    /// </summary>
    public static class HU_Functions
    {
        const string VALID_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ 0123456789.-'";

        public static Vector3 StringToVector3(string sVector)
        {
            Vector3 result = Vector3.zero;
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")")) { sVector = sVector.Substring(1, sVector.Length - 2); }
            else
            {
                Debug.Log("[HU_Functions.StringToVector3] string not properly formatted. Required: (x,y,z). Given: " + sVector);
                return result;
            }
            // split the items
            string[] sArray = sVector.Split(',');

            if (sArray.Length != 3)
            {
                Debug.Log("[HU_Functions.StringToVector3] string not properly formatted. Required: (x,y,z). Given: " + sVector);
                return result;
            }
            // store as a Vector3
            result = new Vector3( float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]));
            return result;
        }

        public static Quaternion StringToQuaternion(string sQuaternion)
        {
            Quaternion result = new Quaternion();
            // Remove the parentheses
            if (sQuaternion.StartsWith("(") && sQuaternion.EndsWith(")"))
            {
                sQuaternion = sQuaternion.Substring(1, sQuaternion.Length - 2);
            }
            else
            {
                Debug.Log("[HU_Functions.StringToQuaternion] string not properly formatted. Required: (x,y,z). Given: " + sQuaternion);
                return result;
            }
            // split the items
            string[] sArray = sQuaternion.Split(',');
            
            if (sArray.Length != 3)
            {
                Debug.Log("[HU_Functions.StringToVector3] string not properly formatted. Required: (x,y,z). Given: " + sQuaternion);
                return result;
            }
            // store as a Quaternion
            result = new Quaternion( float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]), float.Parse(sArray[3]));
            return result;
        }

        public static bool IntToBool(int i) { return i != 0; }

        public static int BoolToInt(bool i) { return i ? 1 : 0; }

        /// <summary>
        /// Returns true if string only contains characters a-z, A-Z, 0-9 ' ', '.', and '-'.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ValidInput(string input)
        {
            return !input.Contains(".txt") && input.All(c => VALID_CHARACTERS.Contains(c.ToString()));
        }

        /// <summary>
        /// Modifies the text of an TMP_InputField so that it is a number and is greater than or equal to lowerBound (pair.key) and lesser than or equal to upperBound (pair.value).
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="pair"></param>
        public static void SanitizeIntIP(ref TMP_InputField ip, KeyValuePair<float, float> pair)
        {
            if (!ip) { Debug.Log("[HU_Functions.SanitizeInput()] inputField is null");}
            
            if (ip.text == "" && ip.gameObject != EventSystem.current.currentSelectedGameObject || ip.text == "-") ip.text = pair.Key.ToString();
            if (ip.text != "") {if (float.Parse(ip.text) < pair.Key) { ip.text = pair.Key.ToString(); }}
            
            if (ip.text == ""){ return;}
            if (float.Parse(ip.text) > pair.Value) { ip.text = pair.Value.ToString();}
        }
        
        /// <summary>
        /// Modifies the text of an TMP_InputField so that it is a number and is greater than or equal to lowerBound (pair.key) and lesser than or equal to upperBound (pair.value).
        /// </summary>
        /// <param name="ip"></param>
        public static void SanitizeIntIP(ref TMP_InputField ip, KeyValuePair<int, int> pair)
        {
            if (!ip) { Debug.Log("[HU_Functions.SanitizeInput()] inputField is null");}
            
            if (ip.text == "" && ip.gameObject != EventSystem.current.currentSelectedGameObject || ip.text == "-") ip.text = pair.Key.ToString();
            if (ip.text != "") {if (int.Parse(ip.text) < pair.Key) { ip.text = pair.Key.ToString(); }}
            
            if (ip.text == ""){ return;}
            if (int.Parse(ip.text) > pair.Value) { ip.text = pair.Value.ToString();}
        }

        /// <summary>
        /// Converts a Dictionary (string, string) to a json formatted string. 
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string Dict_To_JSON(Dictionary<string, string> dict)
        {
            string json = "{\n";
            foreach (KeyValuePair<string, string> pair in dict)
            {
                json += "\"" + pair.Key;
                json += "\": \"";
                json += pair.Value;
                json += "\",\n";
            }
            json = json.Remove(json.Length - 2);
            json += "\n}";
            return json;
        }

        /// <summary>
        /// Converts a json formatted string to a Dictionary (string, string). Does not handle nested dictionaries. 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, string> JSON_To_Dict(string json)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (json.Contains("["))
            {
                Debug.Log("[HU_Functions.JSON_To_Dict] Cannot handle given format. Format must be {\n/t\"Key\": \"Value\", \n/t\"Key\": \"Value\"\n}");
                return dict;
            }
            string key = "";
            string value = "";
            bool addingToPair = false;
            bool addingToKey = false;
            foreach (char t in json)
            {
                // Is this the beginning or end of a json key or value?
                if (t == '\"')
                {
                    if (addingToPair)
                    {
                        Add_JSON_Entry_To_Dict(out addingToPair, ref addingToKey, ref key, ref value, ref dict);
                        continue;
                    }
                    addingToKey = !addingToKey;
                    addingToPair = true; 
                    continue; 
                }
                if(addingToPair) { Add_Char_To_JSON_Key_Value_Pair(t, ref addingToKey, ref key, ref value);}
            }
            return dict;
        }

        /// <summary>
        /// Returns a position, a moving object must move towards in order to intercept another moving object. 
        /// </summary>
        /// <param name="shooterPosition"></param>
        /// <param name="shooterVelocity"></param>
        /// <param name="shotSpeed"></param>
        /// <param name="targetPosition"></param>
        /// <param name="targetVelocity"></param>
        /// <returns></returns>
        public static Vector3 FirstOrderIntercept
            (ref Vector3 shooterPosition, ref Vector3 shooterVelocity, ref float shotSpeed, ref Vector3 targetPosition, ref Vector3 targetVelocity)
        {
            Vector3 targetRelativePosition = targetPosition - shooterPosition;
            Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
            float t = FirstOrderInterceptTime(ref shotSpeed, ref targetRelativePosition, ref targetRelativeVelocity);
            return targetPosition + t * (targetRelativeVelocity);
        }
        
        static void Add_JSON_Entry_To_Dict(out bool addingToPair, ref bool addingToKey,ref string key, ref string value, ref Dictionary<string, string> dict)
        {
            addingToPair = false;
            if (addingToKey) { return; }
            dict[key] = value;
            key = value = "";
        }

        static void Add_Char_To_JSON_Key_Value_Pair(char t, ref bool addingToKey,ref string key, ref string value)
        {
            if (addingToKey) { key += t; return; }
            value += t;
        }

        //first-order intercept using relative target position
        static float FirstOrderInterceptTime(ref float shotSpeed, ref Vector3 targetRelativePosition, ref Vector3 targetRelativeVelocity)
        {
            float velocitySquared = targetRelativeVelocity.sqrMagnitude;
            if (velocitySquared < 0.001f) return 0f;

            float a = velocitySquared - shotSpeed * shotSpeed;

            //handle similar velocities
            if (Mathf.Abs(a) < 0.001f) { return Mathf.Max( -targetRelativePosition.sqrMagnitude / (2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition)), 0f); }

            float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
            float c = targetRelativePosition.sqrMagnitude;
            float determinant = b * b - 4f * a * c;

            if (!(determinant > 0f)) return determinant < 0f ? 0f : Mathf.Max(-b / (2f * a), 0f); //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a), t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            return  (t1 > 0f) ?  ((t2 > 0f) ? Mathf.Min(t1, t2) : t1) : Mathf.Max(t2, 0f); //don't shoot back in time
        }
        
        //FirstOrderIntercept / FirstOrderInterceptTime
        //The MIT License(MIT)

        //Copyright(c) 2008 Daniel Brauer

        //Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files(the "Software"), 
        //    to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
        //        and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

        //The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

        //THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
        //FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
        //WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    }
}