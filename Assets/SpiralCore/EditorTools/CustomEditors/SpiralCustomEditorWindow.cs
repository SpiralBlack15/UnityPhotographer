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
    public class SpiralCustomEditorWindow : EditorWindow
    {
        private MonoScript m_script = null;
        protected MonoScript monoScript { get { return this.CashedMono(ref m_script); } }

        protected Color colorDefault = Color.white;

        protected void OnEnable()
        {
            colorDefault = GUI.color;
        }

        protected void OpenStandartBack(Color? color = null, bool includeLogo = true, bool includeScript = true)
        {
            EditorGUILayout.Space();
            if (color == null) SpiralEditor.BeginPanel(GroupType.Vertical);
            else SpiralEditor.BeginPanel(GroupType.Vertical, (Color)color);
            if (includeLogo) SpiralEditor.DrawLogoLine();
            if (includeScript) SpiralEditor.DrawScriptField(monoScript, "Editor");
        }

        protected void CloseStandartBack()
        {
            SpiralEditor.EndPanel();
        }

        protected static T GetRefInstance<T>(ref T m_instance) where T : SpiralCustomEditorWindow
        {
            if (m_instance == null) m_instance = GetWindow(typeof(T)) as T;
            return m_instance;
        }

        protected static T GetInstance<T>() where T : SpiralCustomEditorWindow
        {
            return GetWindow(typeof(T)) as T;
        }

        protected static T StandartInit<T>() where T : SpiralCustomEditorWindow
        {
            var window = GetWindow(typeof(T)) as T;
            window.Show();
            return window;
        }
    }
}
#endif
