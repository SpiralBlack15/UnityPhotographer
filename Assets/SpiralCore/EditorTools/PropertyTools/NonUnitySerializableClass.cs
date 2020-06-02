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

namespace Spiral.EditorToolkit
{
    /// <summary>
    /// Для тех кастомных классов, которые могут быть сериализованы и нуждаться в валидации
    /// </summary>
    public abstract class NonUnitySerializableClass
    {
        /// <summary>
        /// Необходимо эту функцию, если дочерний объект может быть сериализован в составе массива
        /// Причина: при создании нового элемента массива ВСЕ его значения ставятся на дефолтные,
        /// даже те, которые определены в конструкторе класса. Нет, писать их значение по умолчанию
        /// не поможет. Нет, переопределение переменных в составе класса не поможет.
        /// Выход - кидать Validation, иначе всё накроется медным тазом.
        /// </summary>
        protected virtual void DefaultEditorObject()
        {
            // virtually do nothing
        }

        [SerializeField]private bool validated = false;
        public void EditorCreated(bool force = false)
        {
            if (validated && !force) return;
            DefaultEditorObject();
            validated = true;
        }

        public NonUnitySerializableClass()
        {
            EditorCreated(true);
            // прокидываем, чтобы для новосозданных нормальным путём объектов
            // не происходила некая дичь
        }
    }
}
