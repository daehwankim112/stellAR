//======= Copyright (c) Vaki OY, All rights reserved. ===============

using UnityEngine;


namespace VRConsole
{
    public class VRConsoleExampleSceneErrorProducer : MonoBehaviour
    {

        public LogType logType;

        private void OnMouseDown()
        {
            switch (logType)
            {
                case LogType.Error:
                    Debug.LogError("This is a custom error generated at Error Producer This is a custom error generated at Error Producer This is a custom error generated at Error Producer This is a custom error generated at Error Producer This is a custom error generated at Error Producer This is a custom error generated at Error Producer");
                    break;
                case LogType.Warning:
                    Debug.LogWarning("This is a custom warning generated at Error Producer");
                    break;
                case LogType.Log:
                    Debug.Log("This is a custom debug message generated at Error Producer");
                    break;
            }
        }
    }
}
