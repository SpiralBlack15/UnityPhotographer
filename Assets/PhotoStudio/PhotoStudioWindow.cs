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
using Spiral.Core;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using Unity.EditorCoroutines.Editor;
namespace Spiral.EditorToolkit.PhotoStudio
{
    public class PhotoStudioWindow : SpiralCustomEditorWindow
    {
        private static PhotoStudioWindow m_instance = null;
        public static PhotoStudioWindow instance { get { return GetRefInstance(ref m_instance); } }

        [SerializeField]private string m_path = "";
        /// <summary>
        /// Путь, в который мы сохраняемся
        /// </summary>
        public string path // can be changed outside environment
        {
            get { return m_path; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = SpiralEditorTools.GetDirectory("");
                m_path = value;
            }
        }

        [SerializeField]private string m_namePrefix = "shot_";
        /// <summary>
        /// Префикс имени файла
        /// </summary>
        public string namePrefix 
        {
            get { return m_namePrefix; }
            set { m_namePrefix = value; }
        }

        [SerializeField]private bool m_useNativeResolution = false;
        /// <summary>
        /// Использовать разрешение, выставленное в GameView (рекомендуется).
        /// Если нет, то будет использовано заданное вами фиксированное разрешение,
        /// однако каждый скриншот будет приводить к некритичной ошибке в OnGUI.
        /// </summary>
        public bool useNativeResolution
        {
            get { return m_useNativeResolution; }
            set { m_useNativeResolution = value; }
        }

        [SerializeField]private Vector2Int m_fixedResolution = new Vector2Int(1024, 768);
        /// <summary>
        /// Фиксированное разрешение на тот случай, 
        /// если вы не используете нативное разрешение окна GameView
        /// </summary>
        public Vector2Int fixedResolution
        {
            get { return m_fixedResolution; }
            set
            {
                value.x = value.x.ClampLow(16);
                value.y = value.y.ClampLow(16);
                m_fixedResolution = value;
            }
        }

        [SerializeField]private bool m_makeScreenOnGameStart = true;
        /// <summary>
        /// Делать скриншот на начале игры
        /// </summary>
        public bool makeScreenShotOnGameStart
        {
            get { return m_makeScreenOnGameStart; }
            set { m_makeScreenOnGameStart = value; }
        }

        [SerializeField]private float m_secondsAfter = 15;
        /// <summary>
        /// Скриншот на начале игры делается через N секунд после старта
        /// </summary>
        public float secondsAfter
        {
            get { return m_secondsAfter; }
            set { m_secondsAfter = value.ClampLow(0.1f); }
        }

        public Texture2D lastSnapshot { get; private set; } = null;

        private Vector2 scrollPos;
        private RenderTexture gameViewRenderResult = null;

        [MenuItem("Spiral Tools/PhotoStudio")]
        public static void Init()
        {
            m_instance = StandartInit<PhotoStudioWindow>();
        }

        // UNITY FUNCTIONS ========================================================================
        // Стандартные функции юнити
        //=========================================================================================
        private new void OnEnable()
        {
            base.OnEnable();
            titleContent.text = "PhotoStudio"; 
            
            EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private void OnDestroy()
        {
            EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
            if (lastSnapshot != null) lastSnapshot.KillTexture();
        }

        private void OnGUI()
        {
            var scrollViewOption = GUILayout.Height(position.height);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, scrollViewOption);
            OpenStandartBack();
            DrawSettings();
            DrawDebugPhotos();
            DrawSnapshotPanel();
            CloseStandartBack();
            EditorGUILayout.EndScrollView();

            if (shot)
            {
                MakeShot(true);
                shot = false;
                waiting = false;
            }
        }

        // AFTER START PHOTO ======================================================================
        // Фотка через несколько секунд после старта
        //=========================================================================================
        private void EditorApplication_playModeStateChanged(PlayModeStateChange stateChange)
        {
            if (stateChange != PlayModeStateChange.EnteredPlayMode) return;
            if (makeScreenShotOnGameStart)
            {
                EditorCoroutineUtility.StartCoroutine(WaitForGame(), this);
            }
        }

        [NonSerialized]private bool shot = false;
        [NonSerialized]private bool waiting = false;
        private IEnumerator WaitForGame()
        {
            while (Time.time < secondsAfter)
            {
                yield return null;
            }
            EditorCoroutineUtility.StartCoroutine(WaitForEditor(), this);
        }

        private IEnumerator WaitForEditor()
        {
            waiting = true;
            int ticks = 0;
            while (ticks < 10)
            {
                ticks++;
                yield return null;
            }
            shot = true;
            waiting = false;
            Repaint(); // провоцируем перерисовку, чтобы сделать вывзов
        }

        public void DoSnapshot()
        {
            EditorCoroutineUtility.StartCoroutine(WaitForEditor(), this);
        }

        // MAKE A PHOTO ===========================================================================
        // Делаем фотки нашего GameView
        //=========================================================================================
        private void MakeShot(bool save)
        {
            try
            {
                if (lastSnapshot != null)
                {
                    lastSnapshot.KillTexture();
                }

                if (useNativeResolution)
                {
                    gameViewRenderResult = GameViewUtils.GetRenderTextureDirect();
                }
                else
                {
                    gameViewRenderResult = GameViewUtils.GetGameViewSnapshotUnsafe(fixedResolution);
                }

                if (gameViewRenderResult != null)
                {
                    lastSnapshot = gameViewRenderResult.GetTexture2D();
                    lastSnapshot.FlipYSave();
                    if (lastSnapshot != null && save)
                    {
                        string name = namePrefix + GetTimeStr();
                        lastSnapshot.SaveTexture(path, name);
                    }
                    gameViewRenderResult.KillTexture();
                }
            }
            catch (Exception error) 
            {
                Debug.LogError(error);
            }
        }

        // DRAW INSPECTOR WINDOW ==================================================================
        // Рисуем инспектор
        //=========================================================================================
        private void DrawSettings()
        {
            SpiralEditor.BeginPanel("Settings");
            EditorGUI.indentLevel += 1;

            path = SpiralEditor.SelectPathField("Where to save", path);
            namePrefix = EditorGUILayout.TextField("Name prefix", namePrefix);
            useNativeResolution = EditorGUILayout.Toggle("Use native resolution", useNativeResolution);
            if (!useNativeResolution)
            {
                EditorGUI.indentLevel += 1;
                fixedResolution = EditorGUILayout.Vector2IntField("Fixed resolution", fixedResolution);
                EditorGUI.indentLevel -= 1;
                if (fixedResolution.x > 2000 || fixedResolution.y > 2000)
                {
                    SpiralEditor.MessagePanel("High resolution screenshot can freeze Unity for some seconds!", MessageType.Warning);
                }
            }

            EditorGUI.indentLevel -= 1;
            SpiralEditor.EndPanel();
        }

        private void DrawDebugPhotos()
        {
            SpiralEditor.BeginPanel("Debug Photos");
            EditorGUI.indentLevel += 1;

            makeScreenShotOnGameStart = EditorGUILayout.Toggle("Screenshot on start", makeScreenShotOnGameStart);
            if (makeScreenShotOnGameStart)
            {
                EditorGUI.indentLevel += 1;
                secondsAfter = EditorGUILayout.FloatField("After (.s)", secondsAfter);
                EditorGUI.indentLevel -= 1;
            }

            EditorGUI.indentLevel -= 1;
            SpiralEditor.EndPanel();
        }

        private void DrawSnapshotPanel()
        {
            SpiralEditor.BeginPanel("Last snapshot preview");
            SpiralEditor.SetGUIEnabled(false);
            SpiralEditor.DrawObjectField("Snapshot", lastSnapshot, false);
            if (lastSnapshot != null) SpiralEditor.BoldLabel($"{lastSnapshot.width}x{lastSnapshot.height}", true);
            SpiralEditor.RestoreGUIEnabled();

            bool snapShotEnabled = GameViewUtils.gameView != null && !waiting;
            SpiralEditor.SetGUIEnabled(snapShotEnabled);
            try
            {
                bool makescreen = SpiralEditor.CenteredButton("Make a photo");
                if (makescreen)
                {
                    EditorCoroutineUtility.StartCoroutine(WaitForEditor(), this);
                }
            }
            catch
            {
                SpiralEditor.EndPanel();
                return;
            } // welcome to the silence null
            SpiralEditor.RestoreGUIEnabled();
            if (!snapShotEnabled)
            {
                if (waiting)
                {
                    SpiralEditor.MessagePanel("Wait...", MessageType.Info);
                }
                else
                {
                    SpiralEditor.MessagePanel("If disabled, try to open or restart GameView", MessageType.Warning);
                }
            }
            SpiralEditor.EndPanel();
        }

        // UTILS ==================================================================================
        // Утилитки
        //=========================================================================================
        private static string TimeZero(string input)
        {
            if (input.Length == 1) input = "0" + input;
            return input;
        }

        private static string GetTimeStr()
        {
            DateTime utc  = DateTime.UtcNow;
            string year   = utc.Year.ToString();
            string month  = TimeZero(utc.Month.ToString());
            string day    = TimeZero(utc.Day.ToString());
            string hour   = TimeZero(utc.Hour.ToString());
            string minute = TimeZero(utc.Minute.ToString());
            string second = TimeZero(utc.Second.ToString());

            string answer = $"{year}'{month}'{day}_{hour}'{minute}'{second}";
            return answer;
        }
    }
}
#endif