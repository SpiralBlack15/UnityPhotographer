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

using Spiral.Core;
using System;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Spiral.EditorToolkit
{
    public static class GameViewUtils // TODO: добавить возможность вырубать Gizmo
    {
        /// <summary>
        /// Всё закешировано
        /// </summary>
        public static bool cashed { get; private set; } = false;

        private static Type m_typeGameView = null;
        /// <summary>
        /// Тип окна GameView
        /// </summary>
        public static Type typeGameView
        {
            get
            {
                if (!cashed) GetGameView(); // автоматически найдёт
                return m_typeGameView;
            }
        }

        private static MethodInfo m_methodRenderView = null;
        /// <summary>
        /// Открывает доступ к методу RenderView, позволяющему считать конечное изображение из GameView в RenderTexture 
        /// </summary>
        private static MethodInfo methodRenderView
        {
            get
            {
                if (!cashed) GetGameView();
                return m_methodRenderView;
            }
        }

        private static PropertyInfo m_propertyTargetSize = null;
        /// <summary>
        /// Доступ к свойству, отвечающему за target size у RenderView
        /// </summary>
        private static PropertyInfo propertyTargetSize
        {
            get
            {
                if (!cashed) GetGameView();
                return m_propertyTargetSize;
            }
        }

        private static FieldInfo m_fieldRenderTexture = null;
        private static FieldInfo fieldRenderTexture
        {
            get
            {
                if (!cashed) GetGameView();
                return m_fieldRenderTexture;
            }
        }

        /// <summary>
        /// Выставляем targetSize (внимание: через рефлексию и медленно)
        /// </summary>
        public static Vector2 gameViewTargetSize
        {
            get
            {
                if (gameView == null) return Vector2.zero;
                return (Vector2)propertyTargetSize.GetValue(gameView);
            }
            set
            {
                if (gameView == null) return;
                propertyTargetSize.SetValue(gameView, value); 
            }
        }

        private static EditorWindow m_gameView = null;
        /// <summary>
        /// Окошко GameView, если открыто
        /// </summary>
        public static EditorWindow gameView
        {
            get
            {
                if (m_gameView == null) GetGameView();
                return m_gameView;
            }
        }

        /// <summary>
        /// Взять текущий главный GameView
        /// </summary>
        /// <returns></returns>
        public static EditorWindow GetGameView()
        {
            EditorWindow editorWindow = null;
            try
            {
                editorWindow = EditorWindow.GetWindow<EditorWindow>("Game");
            }
            catch { } // welcome to silent null

            if (editorWindow == null) return null;
            
            if (editorWindow.GetType().Name != "GameView") return null;

            m_gameView = editorWindow;
            
            if (!cashed)
            {
                cashed = true;
                m_typeGameView = m_gameView.GetType();
                m_methodRenderView   = m_typeGameView.GetMethod("RenderView", BindingFlags.NonPublic | BindingFlags.Instance);
                m_propertyTargetSize = m_typeGameView.GetProperty("targetSize", BindingFlags.NonPublic | BindingFlags.Instance);
                m_fieldRenderTexture = m_typeGameView.GetField("m_RenderTexture", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            return m_gameView;
        }

        /// <summary>
        /// Взять скриншот, не меняя параметров.
        /// В большинстве случаев штатно возвращает последний скриншот, поскольку
        /// не пересоздаёт текстуру и не меняет параметры.
        /// </summary>
        /// <returns>Новую текстуру</returns>
        public static RenderTexture GetGameViewSnapshotSafe()
        {
            if (gameView == null)
            {
                Debug.LogWarning("You need game view open to make a snapshot");
                return null;
            }

            Vector2 mouse = Input.mousePosition;
            object[] param = new object[] { mouse, false };
            return (RenderTexture)methodRenderView.Invoke(gameView, param);
        }

        /// <summary>
        /// Берем рендер-текстуру напрямую, содержит последний отренедренный кадр
        /// </summary>
        /// <returns>Рендер-текстура от GameView</returns>
        public static RenderTexture GetRenderTextureDirect()
        {
            if (gameView == null) // вызовет GetGameView() в проверке в любом случае
            {
                Debug.LogWarning("You need game view open to make a snapshot");
                return null;
            }

            gameViewTargetSize = new Vector2(640, 480);
            gameView.Repaint();

            return fieldRenderTexture.GetValue(gameView) as RenderTexture;
        }

        /// <summary>
        /// Взять скриншот, выставив разрешение.
        /// Внимание: скриншот будет сделан в нужном разрешении штатно, но в консоли
        /// будет ошибка GUI (никак не влияет на устойчивость редактора). Здесь идёт
        /// подмена текущего ивента в юнити, чтобы вызвать принудительный рендер.
        /// Также может дать чёрный скриншот, если вызвана не из-под OnGUI
        /// </summary>
        /// <param name="resolution">Разрешение</param>
        /// <returns>Скриншот GameView в целевом разрешении</returns>
        public static RenderTexture GetGameViewSnapshotUnsafe(Vector2 resolution)
        {
            Debug.Log($"Force snapshot {resolution.x}x{resolution.y}");
            if (gameView == null) // вызовет GetGameView()
            {
                Debug.LogWarning("You need game view open to make a snapshot");
                return null;
            }
            resolution.x = resolution.x.ClampLow(16);
            resolution.y = resolution.y.ClampLow(16);
            gameViewTargetSize = resolution;

            RenderTexture rt;
            // подменяем текущее событие при необходимости
            Event prev = null;
            if (Event.current == null)
            {
                Event.current = new Event { type = EventType.Repaint };
            }
            else if (Event.current.type != EventType.Repaint)
            {
                prev = new Event(Event.current);
                Event.current.type = EventType.Repaint;
            }
            object[] param = new object[] { Vector2.zero, true };
            rt = (RenderTexture)methodRenderView.Invoke(gameView, param);

            // вертаем назад старое событие
            Event.current = prev;

            return rt;
        }
    }
}
#endif