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

using System.Collections.Generic;
using UnityEngine;

namespace Spiral.Core
{
    public static class CleanerTools
    {
        public static void KillObject(this Object obj, bool undo = true)
        {
#if UNITY_EDITOR
            if (undo) Undo(obj, "Cleaner Call");
#endif
            if (Application.isPlaying) Object.Destroy(obj); else Object.DestroyImmediate(obj);
        }

        private static void KillComponentGameObject<T>(this T obj) where T : Component
        {
            if (obj == null) return;
            KillObject(obj.gameObject, true);
        }

        public static void KillComponent(this Component component, bool killObject = false)
        {
            if (component == null) return;

            if (killObject) KillObject(component.gameObject, true);
            else KillObject(component, true);
        }

        public static void KillGameObject(this GameObject gameObject)
        {
            KillObject(gameObject, true);
        }

#if UNITY_EDITOR 
        public static void Undo(this Object obj, string undoComment)
        {
            if (obj == null) return;
            UnityEditor.Undo.RecordObject(obj, undoComment);
        }

        public static void UndoLast()
        {
            UnityEditor.Undo.PerformUndo();
        }
#endif

        /// <summary>
        /// Убивает лист компонент
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void KillComponentList<T>(this List<T> list) where T : Component
        {
            int count = list.Count;
            if (count == 0) return;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    KillComponentGameObject(list[i]);
                }
                catch
                {
                    Debug.Log("Object cannot be deleted");
                }
            }
            list.Clear();
        }

        /// <summary>
        /// Разрушает все объекты в листе
        /// </summary>
        /// <typeparam name="T">Тип объектов</typeparam>
        /// <param name="list">Лист объектов под уничтожение</param>
        public static void KillObjectList<T>(this List<T> list) where T : Object
        {
            int count = list.Count;
            if (count == 0) return;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    KillObject(list[i], true);
                }
                catch
                {
                    Debug.Log("Object cannot be deleted");
                }
            }
            list.Clear();
        }

    }
}