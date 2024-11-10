using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRConsole
{
    [ExecuteInEditMode]
    public class VRConsoleMessageUIResizer : MonoBehaviour
    {
        public RectTransform RootCanvasRectTransform;
        public GridLayoutGroup LayoutGroup;
        public BoxCollider ConsoleMessageBoxCollider;
        public RectTransform StackTraceTextRectTransform;

        void Update()
        {
                LayoutGroup.cellSize = new Vector2(RootCanvasRectTransform.sizeDelta.x - 10, LayoutGroup.cellSize.y);
                ConsoleMessageBoxCollider.size = new Vector2(RootCanvasRectTransform.sizeDelta.x - 10, ConsoleMessageBoxCollider.size.y);
                StackTraceTextRectTransform.sizeDelta = new Vector2(RootCanvasRectTransform.sizeDelta.x - 10, StackTraceTextRectTransform.sizeDelta.y);
        }
    }
}
