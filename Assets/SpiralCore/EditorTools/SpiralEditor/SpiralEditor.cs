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

using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
namespace Spiral.EditorToolkit
{
    public static partial class SpiralEditor
    {
        private static bool prevGUIEnabled = true;
        public static bool SetGUIEnabled(bool value)
        {
            prevGUIEnabled = GUI.enabled;
            GUI.enabled = value;
            return prevGUIEnabled;
        }

        public static void RestoreGUIEnabled()
        {
            GUI.enabled = prevGUIEnabled;
        }

        // Quick Object Field ---------------------------------------------------------------------
        #region OBJECT FIELD
        public static T DrawObjectField<T>(string content, T obj, bool allowScenePick = true) where T : UnityEngine.Object
        {
            return (T)EditorGUILayout.ObjectField(content, obj, typeof(T), allowScenePick);
        }
        #endregion

        // Path Fields ----------------------------------------------------------------------------
        #region PATH FIELDS
        public static string SelectPathField(string panelName, string path, string ext = "")
        {
            BeginGroup(GroupType.Horizontal);

            if (string.IsNullOrWhiteSpace(path)) path = SpiralEditorTools.GetDirectory(path);

            EditorGUILayout.TextField(GetLabel(panelName, "Path to save screenshots"), path);

            bool pathExists = Directory.Exists(path);
            SetGUIEnabled(pathExists);
            bool open = MiniButton("Open");
            if (open)
            {
                try
                {
                    EditorUtility.RevealInFinder(path);
                }
                catch { }
            }
            RestoreGUIEnabled();

            bool browse = MiniButton("...");
            string ans;
            if (browse)
            {
                string dir = SpiralEditorTools.GetDirectory(path);
                bool folder = string.IsNullOrEmpty(ext);
                ans = folder ?
                      EditorUtility.OpenFolderPanel(panelName, dir, "") :
                      EditorUtility.OpenFilePanel(panelName, dir, ext);
                if (ans == "") ans = path;
            }
            else ans = path;

            EndGroup();
            return ans;
        }
        #endregion

        // Logo -----------------------------------------------------------------------------------
        #region LOGO
        public static void DrawLogoLine(Color? color = null)
        {
            Color defaultColor = GUI.color;
            GUI.color = color != null ? (Color)color : SpiralStyles.defaultLogoColor;
            EditorGUILayout.BeginVertical(SpiralStyles.panel);
            EditorGUILayout.LabelField("SpiralBlack Scripts © 2020", SpiralStyles.labelLogo);
            EditorGUILayout.EndVertical();
            GUI.color = defaultColor;
        }
        #endregion

        // Other ----------------------------------------------------------------------------------
        public static GUIContent GetLabel(string text, string tooltip = "")
        {
            GUIContent output = new GUIContent(text, tooltip);
            return output;
        }
    }
}
#endif

