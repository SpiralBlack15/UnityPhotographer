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
using System;

#if UNITY_EDITOR
using UnityEditor;
namespace Spiral.EditorToolkit
{
    public static partial class SpiralEditor
    {
        private static void DrawScriptFieldOnly(MonoScript monoScript, Type type, string content)
        {
            if (content == "") content = "Script";
            if (monoScript != null)
            {
                _ = EditorGUILayout.ObjectField(content, monoScript, type, false);
            }
            else
            {
                EditorGUILayout.LabelField(content, $"Single file of [{type.Name}] not found", SpiralStyles.panel);
            }
        }

        public static void DrawScriptField(SerializedObject serializedObject, bool forceBlock = false)
        {
            BeginPanel(GroupType.Vertical);
            SerializedProperty prop = serializedObject.FindProperty("m_Script");
            SetGUIEnabled(prop != null && !forceBlock);
            EditorGUILayout.PropertyField(prop, true);
            RestoreGUIEnabled();
            EndPanel();
        }

        public static void DrawScriptField(MonoScript monoScript, string content = "")
        {
            BeginPanel(GroupType.Vertical);
            SetGUIEnabled(false);
            if (content == "") content = "Script";
            EditorGUILayout.ObjectField(content, monoScript, typeof(MonoScript), false);
            RestoreGUIEnabled();
            EndPanel();
        }

        public static void DrawScriptField(Type type, string content = "")
        {
            BeginPanel(GroupType.Vertical);
            SetGUIEnabled(false);
            MonoScript monoScript = SpiralEditorTools.GetMonoScript(type);
            if (content == "") content = "Script";
            EditorGUILayout.ObjectField(content, monoScript, typeof(MonoScript), false);
            RestoreGUIEnabled();
            EndPanel();
        }

        public static void DrawScriptField(ScriptableObject scriptableObject, string content = "")
        {
            BeginPanel(GroupType.Vertical);
            SetGUIEnabled(false);
            Type type = scriptableObject.GetType();
            MonoScript monoScript = MonoScript.FromScriptableObject(scriptableObject);
            DrawScriptFieldOnly(monoScript, type, content);
            RestoreGUIEnabled();
            EndPanel();
        }
    }
}
#endif