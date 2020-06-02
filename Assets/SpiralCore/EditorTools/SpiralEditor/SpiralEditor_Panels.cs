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
using System.Collections.Generic;
using Spiral.Core;

#if UNITY_EDITOR
using UnityEditor;
namespace Spiral.EditorToolkit
{
    public enum GroupType { Vertical, Horizontal }

    public static partial class SpiralEditor
    {
        private static readonly List<GroupType> panelTypesStack = new List<GroupType>();

        public static void BeginGroup(GroupType groupType)
        {
            if (groupType == GroupType.Vertical) EditorGUILayout.BeginVertical();
            else EditorGUILayout.BeginHorizontal();
            panelTypesStack.Add(groupType);
        }

        public static void BeginGroup(GroupType groupType, Color color)
        {
            Color prevColor = GUI.color;
            GUI.color = color;
            if (groupType == GroupType.Vertical) EditorGUILayout.BeginVertical();
            else EditorGUILayout.BeginHorizontal();
            panelTypesStack.Add(groupType);
            GUI.color = prevColor;
        }

        public static void BeginPanel(GroupType groupType, Color? color = null)
        {
            Color prevColor = GUI.color;
            GUI.color = color != null ? (Color)color : SpiralStyles.defaultPanelColor;
            if (groupType == GroupType.Vertical) EditorGUILayout.BeginVertical(SpiralStyles.panel);
            else EditorGUILayout.BeginHorizontal(SpiralStyles.panel);
            panelTypesStack.Add(groupType);
            GUI.color = prevColor;
        }

        public static void BeginPanel(string caption, bool smallCaption, params GUILayoutOption[] options)
        {
            BeginPanel(GroupType.Vertical);
            GUIStyle style = smallCaption ? SpiralStyles.labelSmallBold : SpiralStyles.labelBold;
            EditorGUILayout.LabelField(caption, style, options);
        }

        public static void BeginPanel(string caption, Color color, params GUILayoutOption[] options)
        {
            BeginPanel(GroupType.Vertical, color);
            EditorGUILayout.LabelField(caption, SpiralStyles.labelBold, options);
        }

        public static void BeginPanel(string caption, bool smallCaption = false, Color? color = null, params GUILayoutOption[] options)
        {
            Color sendColor = color != null ? (Color)color : SpiralStyles.defaultPanelColor;
            BeginPanel(GroupType.Vertical, sendColor);
            GUIStyle style = smallCaption ? SpiralStyles.labelSmallBold : SpiralStyles.labelBold;
            EditorGUILayout.LabelField(caption, style, options);
        }

        public static void BeginPanel(GUIContent caption, bool smallCaption = false, Color? color = null, params GUILayoutOption[] options)
        {
            Color sendColor = color != null ? (Color)color : SpiralStyles.defaultPanelColor;
            BeginPanel(GroupType.Vertical, sendColor);
            GUIStyle style = smallCaption ? SpiralStyles.labelSmallBold : SpiralStyles.labelBold;
            EditorGUILayout.LabelField(caption, style, options);
        }

        public static void EndPanel()
        {
            if (panelTypesStack.Count == 0)
            {
                Debug.LogWarning("No panels or groups to close");
                return;
            }
            GroupType panelType = panelTypesStack.GetLast();
            if (panelType == GroupType.Vertical) EditorGUILayout.EndVertical();
            else EditorGUILayout.EndHorizontal();
            panelTypesStack.RemoveLast();
        }

        public static void EndGroup()
        {
            EndPanel();
        }

        public static bool BeginFoldoutGroup(ref bool foldout, GUIContent content, GUIStyle captionStyle = null, bool autoclose = true, bool autostart = true)
        {
            if (autostart) BeginPanel(GroupType.Vertical);
            if (captionStyle == null) captionStyle = SpiralStyles.foldoutIndentedBold;
            foldout = EditorGUILayout.Foldout(foldout, content, true, captionStyle);
            if (!foldout && autoclose)
            {
                EndFoldoutGroup();
            }
            return foldout;
        }

        public static bool BeginFoldoutGroup(ref bool foldout, string content, GUIStyle captionStyle = null, bool autoclose = true, bool autostart = true)
        {
            if (autostart) BeginPanel(GroupType.Vertical);
            if (captionStyle == null) captionStyle = SpiralStyles.foldoutIndentedBold;
            foldout = EditorGUILayout.Foldout(foldout, content, true, captionStyle);
            if (!foldout && autoclose)
            {
                EndFoldoutGroup();
            }
            return foldout;
        }

        public static void EndFoldoutGroup()
        {
            EndPanel(); // TODO: помедетировать над более безопасной идеей
        }

        public static void MessagePanel(string message, MessageType messageType = MessageType.None)
        {
            Color panelColor = GUI.color;
            switch (messageType)
            {
                case MessageType.None:
                    panelColor = GUI.color;
                    break;

                case MessageType.Info:
                    panelColor = new Color(0.0f, 0.8f, 0.8f, 0.1f);
                    break;

                case MessageType.Warning:
                    panelColor = new Color(0.8f, 0.8f, 0.0f, 0.5f);
                    break;

                case MessageType.Error:
                    panelColor = new Color(0.8f, 0.0f, 0.0f, 0.1f);
                    break;
            }
            BeginPanel(GroupType.Vertical, panelColor);
            EditorGUILayout.HelpBox(message, messageType);
            EndPanel();
        }

        public static void BoolPanel(string message, ref bool showHelp, MessageType messageType = MessageType.None)
        {
            BeginPanel(GroupType.Vertical);
            EditorGUI.indentLevel += 1;
            string helpFoldout = showHelp ? "Hide help" : "Show help";
            showHelp = EditorGUILayout.Foldout(showHelp, helpFoldout);
            EditorGUI.indentLevel -= 1;
            if (showHelp)
            {
                EditorGUILayout.HelpBox(message, messageType);
            }
            EndPanel();
        }
    }
}
#endif