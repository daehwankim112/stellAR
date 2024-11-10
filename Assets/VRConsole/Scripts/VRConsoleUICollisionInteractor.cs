//======= Copyright (c) Vaki OY, All rights reserved. ===============

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace VRConsole
{
    public class VRConsoleUICollisionInteractor : MonoBehaviour
    {

        public float submitTickRate = 20f;
        public bool tickIfTriggered;

        private float submitDelaytime;

        private void Start()
        {
            submitDelaytime = 1 / submitTickRate;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "UIActivator")
            {
                PointerEventData pointData = new PointerEventData(EventSystem.current);
                if (GetComponent<Button>() || GetComponent<Toggle>())
                {
                    ExecuteEvents.Execute(gameObject, pointData, ExecuteEvents.submitHandler);
                    ExecuteEvents.Execute(gameObject, pointData, ExecuteEvents.pointerEnterHandler);
                    if (tickIfTriggered)
                    {
                        StartCoroutine(TickSubmit());
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "UIActivator")
            {
                StopAllCoroutines();
                PointerEventData pointData = new PointerEventData(EventSystem.current);
                if (GetComponent<Button>() || GetComponent<Toggle>())
                {
                    ExecuteEvents.Execute(gameObject, pointData, ExecuteEvents.pointerExitHandler);
                }
            }
        }

        private IEnumerator TickSubmit()
        {
            PointerEventData pointData = new PointerEventData(EventSystem.current);
            while (gameObject)
            {
                yield return new WaitForSeconds(submitDelaytime);
                ExecuteEvents.Execute(gameObject, pointData, ExecuteEvents.submitHandler);
            }
        }
    }
}