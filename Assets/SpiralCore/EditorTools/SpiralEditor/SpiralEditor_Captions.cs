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
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
namespace Spiral.EditorToolkit
{
    public static partial class SpiralEditor
    {
        private static GUILayoutOption labelOption = GUILayout.Height(20);

        public static void BoldLabel(GUIContent content, bool selectable = false, bool small = false, params GUILayoutOption[] options)
        {
            GUIStyle style = small ? SpiralStyles.labelSmallBold : SpiralStyles.labelBold;
            if (!selectable) EditorGUILayout.LabelField(content, style, options);
            else
            {
                if (options == null) options = new GUILayoutOption[1] { labelOption };
                else
                {
                    List<GUILayoutOption> options1 = new List<GUILayoutOption>(options)
                    {
                        labelOption
                    };
                    options = options1.ToArray();
                }
                EditorGUILayout.SelectableLabel(content.text, style, options);
            }
        }

        public static void BoldLabel(string content, bool selectable = false, bool small = false, params GUILayoutOption[] options)
        {
            GUIStyle style = small ? SpiralStyles.labelSmallBold : SpiralStyles.labelBold;
            if (!selectable) EditorGUILayout.LabelField(content, style, options);
            else
            {
                if (options == null) options = new GUILayoutOption[1] { labelOption };
                else
                {
                    List<GUILayoutOption> options1 = new List<GUILayoutOption>(options)
                    {
                        labelOption
                    };
                    options = options1.ToArray();
                }
                EditorGUILayout.SelectableLabel(content, style, options);
            }
        }

        public static void BoldLabel(string content, bool small = false, params GUILayoutOption[] options)
        {
            GUIStyle style = small ? SpiralStyles.labelSmallBold : SpiralStyles.labelBold;
            EditorGUILayout.LabelField(content, style, options);
        }

        public static void BoldLabel(GUIContent content, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(content, SpiralStyles.labelBold, options);
        }
    }
}
#endif
