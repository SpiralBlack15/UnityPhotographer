using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spiral.EditorToolkit.PhotoStudio
{
    public class PhotoShot : MonoBehaviour
    {
#if UNITY_EDITOR
        [ContextMenu("Make a shot")]
        public void MakeShot()
        {
            if (PhotoStudioWindow.instance != null)
            {
                PhotoStudioWindow.instance.DoSnapshot(); 
                // технически после этого вы можете взять снапшот из PhotoStudioWindow через lastSnapshot
            }
        }
#endif
    }
}