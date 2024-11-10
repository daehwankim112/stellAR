//======= Copyright (c) Vaki OY, All rights reserved. ===============

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRConsole
{
    public class VRConsoleRuntimeConsole : MonoBehaviour
    {
        [Header("If this is toggled the console will jump to the newest log")]
        public bool JumpToNewestLog = true;
        [Space(10)]

        [Header("These toggles can be used to choose which sort of messages to show by default")]
        public bool ShowInfos = true;
        public bool ShowWarnings = true;
        public bool ShowErrors = true;

        [HideInInspector]
        public Transform scrollViewContent;
        [HideInInspector]
        public GameObject consoleMessagePrefab;
        [HideInInspector]
        public RectTransform messagesContent_RectTransform;
        [HideInInspector]
        public Scrollbar MessagesScrollBar;
        [HideInInspector]
        public RectTransform stackTraceContent_RectTransform;
        [HideInInspector]
        public RectTransform stackTraceText_RectTransform;
        [HideInInspector]
        public Text stackTrace_Text;
        [HideInInspector]
        public Toggle showInfoMessagesToggle, showWarningMessagesToggle, showErrorMessagesToggle;
        [HideInInspector]
        public Text infoMessagesAmount_Text, warningMessagesAmount_Text, errorMessagesAmount_Text;
        [HideInInspector]
        public Image infoToggleEnabledOverlay_Image, warningToggleEnabledOverlay_Image, errorToggleEnabledOverlay_Image;
        [HideInInspector]
        public Button messagesUp_Button, messagesDown_Button;
        [HideInInspector]
        public Button stackTraceUp_Button, stackTraceDown_Button;
        [HideInInspector]
        public Scrollbar messages_ScrollBar, stackTrace_ScrollBar;
        [HideInInspector]
        public Image WholeMessageBackgroundImage;
        [HideInInspector]
        public Text WholeMessageText;

        private List<string> consoleMessages = new List<string>();
        private List<string> messageStackTraces = new List<string>();
        private List<LogType> consoleMessageTypes = new List<LogType>();
        private List<GameObject> instantiatedMessageObjects = new List<GameObject>();

        private const float messagesScrollBarIncrement = 0.02f;
        private const float scrollViewHeight = 90.0f;
        private const float wholeMessageBackGroundExpandConstant = 5f;

        private void Awake()
        {
            BindListeners();
            Application.logMessageReceived += HandleLogMessage;

            showInfoMessagesToggle.isOn = ShowInfos;
            showWarningMessagesToggle.isOn = ShowWarnings;
            showErrorMessagesToggle.isOn = ShowErrors;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLogMessage;
        }

        void HandleLogMessage(string logString, string stackTrace, LogType type)
        {
            consoleMessages.Add(logString);
            messageStackTraces.Add(stackTrace);
            consoleMessageTypes.Add(type);
            InstantiateNewMessageToConsole(logString, stackTrace, type);
        }

        void InstantiateNewMessageToConsole(string message, string messageTrace, LogType logtype)
        {
            GameObject instantiatedMessageObject = Instantiate(consoleMessagePrefab) as GameObject;
            instantiatedMessageObject.transform.SetParent(scrollViewContent, false);
            instantiatedMessageObject.GetComponent<VRConsoleConsoleMessage>().SetMessage(message, messageTrace, logtype);
            instantiatedMessageObject.SetActive(true);
            instantiatedMessageObjects.Add(instantiatedMessageObject);
            UpdateLogTextFieldAmounts();
            UpdateConsoleMessageColliders();

            if (logtype == LogType.Log && !showInfoMessagesToggle.isOn)
                instantiatedMessageObject.SetActive(false);
            else if (logtype == LogType.Warning && !showWarningMessagesToggle.isOn)
                instantiatedMessageObject.SetActive(false);
            else if (logtype == LogType.Error && !showErrorMessagesToggle.isOn)
                instantiatedMessageObject.SetActive(false);

            if (JumpToNewestLog)
            {
                MessagesScrollBar.value = 0;
            }
        }

        void BindListeners()
        {
            showInfoMessagesToggle.onValueChanged.AddListener(ShowInfoMessagesToggled);
            showErrorMessagesToggle.onValueChanged.AddListener(ShowErrorMessagesToggled);
            showWarningMessagesToggle.onValueChanged.AddListener(ShowWarningMessagesToggled);

            messagesUp_Button.onClick.AddListener(MoveMessagesUpButtonPressed);
            messagesDown_Button.onClick.AddListener(MoveMessagesDownButtonPressed);

            stackTraceUp_Button.onClick.AddListener(StackTraceUpButtonPressed);
            stackTraceDown_Button.onClick.AddListener(StackTraceDownButtonPressed);
        }

        public void ClearWholeLog()
        {
            consoleMessages.Clear();
            messageStackTraces.Clear();
            consoleMessageTypes.Clear();
            SetStackTraceText(" ");

            for (int i = 0; i < instantiatedMessageObjects.Count; i++)
            {
                Destroy(instantiatedMessageObjects[i]);
            }
            instantiatedMessageObjects.Clear();
            UpdateLogTextFieldAmounts();
        }

        public void UpdateWholeLog()
        {
            for (int i = 0; i < instantiatedMessageObjects.Count; i++)
            {
                VRConsoleConsoleMessage messageScript = instantiatedMessageObjects[i].GetComponent<VRConsoleConsoleMessage>();

                switch (messageScript.messageLogType)
                {
                    case LogType.Error:
                        if (showErrorMessagesToggle.isOn)
                        {
                            instantiatedMessageObjects[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            instantiatedMessageObjects[i].gameObject.SetActive(false);
                        }
                        break;
                    case LogType.Warning:
                        if (showWarningMessagesToggle.isOn)
                        {
                            instantiatedMessageObjects[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            instantiatedMessageObjects[i].gameObject.SetActive(false);
                        }
                        break;
                    case LogType.Log:
                        if (showInfoMessagesToggle.isOn)
                        {
                            instantiatedMessageObjects[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            instantiatedMessageObjects[i].gameObject.SetActive(false);
                        }
                        break;
                }
            }
            UpdateLogTextFieldAmounts();
            UpdateConsoleMessageColliders();
        }

        void UpdateConsoleMessageColliders()
        {
            for (int i = 0; i < instantiatedMessageObjects.Count; i++)
            {
                float messageRectY = Mathf.Abs(instantiatedMessageObjects[i].GetComponent<RectTransform>().localPosition.y);
                float contentRectTransformY = Mathf.Abs(messagesContent_RectTransform.localPosition.y);

                if(messageRectY > contentRectTransformY && messageRectY < (contentRectTransformY + scrollViewHeight))
                {
                    instantiatedMessageObjects[i].GetComponent<Collider>().enabled = true;
                }else
                {
                    instantiatedMessageObjects[i].GetComponent<Collider>().enabled = false;
                }
            }
        }

        public void UpdateLogTextFieldAmounts()
        {
            int infos = 0;
            int warnings = 0;
            int errors = 0;

            for (int i = 0; i < instantiatedMessageObjects.Count; i++)
            {
                VRConsoleConsoleMessage consoleMessage = instantiatedMessageObjects[i].GetComponent<VRConsoleConsoleMessage>();

                if (consoleMessage.messageLogType == LogType.Log)
                    infos++;
                else if (consoleMessage.messageLogType == LogType.Warning)
                    warnings++;
                else if (consoleMessage.messageLogType == LogType.Error)
                    errors++;
            }

            infoMessagesAmount_Text.text = infos.ToString();
            warningMessagesAmount_Text.text = warnings.ToString();
            errorMessagesAmount_Text.text = errors.ToString();
        }

        public void SetStackTraceText(string stackTrace)
        {
            stackTrace_Text.text = stackTrace;
            StartCoroutine(SetStackTraceContentSizeDeltaAfterOneFrame());
        }

        public void HideWholeMessageTextAndBG()
        {
            WholeMessageBackgroundImage.gameObject.SetActive(false);
        }

        public void SetAndShowWholeDebugMessage(string wholeMessage, Vector3 messageLocation)
        {
            WholeMessageText.text = wholeMessage;
            WholeMessageBackgroundImage.GetComponent<RectTransform>().position = messageLocation;
            StartCoroutine(SetWholeMessageBackgroundSizeDeltaAfterOneFrame());
        }

        IEnumerator SetStackTraceContentSizeDeltaAfterOneFrame()
        {
            yield return new WaitForEndOfFrame();
            stackTraceContent_RectTransform.sizeDelta = new Vector2(stackTraceContent_RectTransform.sizeDelta.x, stackTraceText_RectTransform.sizeDelta.y + 5f);
        }

        IEnumerator SetWholeMessageBackgroundSizeDeltaAfterOneFrame()
        {
            WholeMessageBackgroundImage.gameObject.SetActive(true);
            yield return new WaitForEndOfFrame();            
            RectTransform backgroundRectTrans = WholeMessageBackgroundImage.GetComponent<RectTransform>();
            RectTransform wholeMessageTextRectTrans = WholeMessageText.GetComponent<RectTransform>();
            backgroundRectTrans.sizeDelta = new Vector2(wholeMessageTextRectTrans.sizeDelta.x + wholeMessageBackGroundExpandConstant, wholeMessageTextRectTrans.sizeDelta.y + wholeMessageBackGroundExpandConstant);
        }

        public void ShowErrorMessagesToggled(bool isOn)
        {
            errorToggleEnabledOverlay_Image.gameObject.SetActive(!isOn);   
            UpdateWholeLog();
        }

        public void ShowWarningMessagesToggled(bool isOn)
        {
            warningToggleEnabledOverlay_Image.gameObject.SetActive(!isOn);
            UpdateWholeLog();
        }

        public void ShowInfoMessagesToggled(bool isOn)
        {
            infoToggleEnabledOverlay_Image.gameObject.SetActive(!isOn);
            UpdateWholeLog();
        }

        public void MoveMessagesUpButtonPressed()
        {
            messages_ScrollBar.value += messagesScrollBarIncrement;
            UpdateConsoleMessageColliders();
        }

        public void MoveMessagesDownButtonPressed()
        {
            messages_ScrollBar.value -= messagesScrollBarIncrement;
            UpdateConsoleMessageColliders();
        }

        public void StackTraceUpButtonPressed()
        {
            stackTrace_ScrollBar.value += messagesScrollBarIncrement;
            UpdateConsoleMessageColliders();
        }

        public void StackTraceDownButtonPressed()
        {
            stackTrace_ScrollBar.value -= messagesScrollBarIncrement;
            UpdateConsoleMessageColliders();
        }
    }
}
