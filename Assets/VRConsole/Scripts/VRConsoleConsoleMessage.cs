//======= Copyright (c) Vaki OY, All rights reserved. ===============

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace VRConsole
{
    public class VRConsoleConsoleMessage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        //[HideInInspector]
        public Image messageIcon_Image;
        //[HideInInspector]
        public Text messageText_Text;
        //[HideInInspector]
        public Button showStackTrace_Button;
        //[HideInInspector]
        public Sprite infoSprite, warningSprite, errorSprite;
        //[HideInInspector]
        public Color infoColor, warningColor, errorColor;
        //[HideInInspector]
        public LogType messageLogType;

        private string thisMessageStackTrace;
        private string wholeMessage;
        private bool hovering;
        private float timeHovered;

        private const float hoverTimeRequiredToShowWholeMessage = 0.5f;

        private void Update()
        {
            if(hovering)
            {
                if(timeHovered > hoverTimeRequiredToShowWholeMessage)
                {
                    GetComponentInParent<VRConsoleRuntimeConsole>().SetAndShowWholeDebugMessage(wholeMessage,transform.position);
                    hovering = false;
                }else
                {
                    timeHovered += Time.deltaTime;
                }
            }
        }

        public void SetMessage(string message, string stackTrace, LogType messageType)
        {
            messageText_Text.text = message;
            messageLogType = messageType;
            wholeMessage = message;
            if (stackTrace == "") //if our stacktrace message is null we are going to use our normal message as our stack trace message. This is the case at least in some Unity warnings like negative scale
            {
                thisMessageStackTrace = message;
            }
            else
            {
                thisMessageStackTrace = stackTrace;
            }

            switch (messageType)
            {
                case LogType.Error:
                    messageIcon_Image.sprite = errorSprite;
                    messageIcon_Image.color = errorColor;
                    break;
                case LogType.Assert:
                    messageIcon_Image.color = Color.magenta;
                    break;
                case LogType.Warning:
                    messageIcon_Image.sprite = warningSprite;
                    messageIcon_Image.color = warningColor;
                    break;
                case LogType.Log:
                    messageIcon_Image.sprite = infoSprite;
                    messageIcon_Image.color = infoColor;
                    break;
                case LogType.Exception:
                    messageIcon_Image.color = Color.cyan;
                    break;
            }

            showStackTrace_Button.onClick.AddListener(ShowStackTraceOnConsole);
        }

        void ShowStackTraceOnConsole()
        {
            GetComponentInParent<VRConsoleRuntimeConsole>().SetStackTraceText(thisMessageStackTrace);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovering = false;
            timeHovered = 0f;
            GetComponentInParent<VRConsoleRuntimeConsole>().HideWholeMessageTextAndBG();
        }
    }
}
