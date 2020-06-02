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

#if UNITY_EDITOR
using UnityEditor;
namespace Spiral.EditorToolkit
{
    public static partial class SpiralEditor
    {
        public static bool Button(string content, GUIStyle style, params GUILayoutOption[] options)
        {
            Color prevColor = GUI.color;
            GUI.color = SpiralStyles.defaultButtonColor;
            if (style == null) style = SpiralStyles.buttonNormal;
            bool result = GUILayout.Button(content, style, options);
            GUI.color = prevColor;
            return result;
        }

        public static bool Button(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            Color prevColor = GUI.color;
            GUI.color = SpiralStyles.defaultButtonColor;
            if (style == null) style = SpiralStyles.buttonNormal;
            bool result = GUILayout.Button(content, style, options);
            GUI.color = prevColor;
            return result;
        }

        public static bool Button(string content, params GUILayoutOption[] options)
        {
            Color prevColor = GUI.color;
            GUI.color = SpiralStyles.defaultButtonColor;
            bool result = GUILayout.Button(content, options);
            GUI.color = prevColor;
            return result;
        }

        public static bool Button(GUIContent content, params GUILayoutOption[] options)
        {
            Color prevColor = GUI.color;
            GUI.color = SpiralStyles.defaultButtonColor;
            bool result = GUILayout.Button(content, options);
            GUI.color = prevColor;
            return result;
        }

        public static bool Button(string content, Color color, GUIStyle style = null, params GUILayoutOption[] options)
        {
            Color prevColor = GUI.color;
            GUI.color = color;
            if (style == null) style = SpiralStyles.buttonNormal;
            bool result = GUILayout.Button(content, style, options);
            GUI.color = prevColor;
            return result;
        }

        public static bool Button(GUIContent content, Color color, GUIStyle style = null, params GUILayoutOption[] options)
        {
            Color prevColor = GUI.color;
            GUI.color = color;
            if (style == null) style = SpiralStyles.buttonNormal;
            bool result = GUILayout.Button(content, style, options);
            GUI.color = prevColor;
            return result;
        }

        public static bool CenteredButton(string content, float width = 150, GUIStyle style = null)
        {
            BeginGroup(GroupType.Horizontal);
            EditorGUILayout.Space();
            bool button = Button(content, style, GUILayout.Width(width));
            EditorGUILayout.Space();
            EndGroup();
            EditorGUILayout.Space();
            return button;
        }

        public static bool CenteredButton(GUIContent content, float width = 150, GUIStyle style = null)
        {
            BeginGroup(GroupType.Horizontal);
            EditorGUILayout.Space();
            bool button = Button(content, style, GUILayout.Width(width));
            EditorGUILayout.Space();
            EndGroup();
            EditorGUILayout.Space();
            return button;
        }

        public static bool CenteredButton(string content, Color color, float width = 150, GUIStyle style = null)
        {
            BeginGroup(GroupType.Horizontal);
            EditorGUILayout.Space();
            bool button = Button(content, color, style, GUILayout.Width(width));
            EditorGUILayout.Space();
            EndGroup();
            EditorGUILayout.Space();
            return button;
        }

        public static bool CenteredButton(GUIContent content, Color color, float width = 150, GUIStyle style = null)
        {
            BeginGroup(GroupType.Horizontal);
            EditorGUILayout.Space();
            bool button = Button(content, color, style, GUILayout.Width(width));
            EditorGUILayout.Space();
            EndGroup();
            EditorGUILayout.Space();
            return button;
        }

        public static bool MiniButton(string content)
        {
            GUIContent btnBrowse = EditorGUIUtility.TrTextContent(content); // TODO: добавь потом тут красивостей
            Vector2 btnSize = SpiralStyles.buttonMini.CalcSize(btnBrowse);
            GUILayoutOption option = GUILayout.MaxWidth(btnSize.x);
            return GUILayout.Button(btnBrowse, SpiralStyles.buttonMini, option);
        }
    }
}
#endif
