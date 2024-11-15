using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MenuGazeSelector : MonoBehaviour {
    public GameObject nextButton;
    public GameObject startButton;
    public GameObject gameMenu;
    public GameObject headTiltMenu;
    public GameObject cursor;
    public Transform centerEyeTransform;
    public float cosineAngle;
    public float Timer = 0.0f;
    public int gazeTime = 3;


    public event EventHandler StartEvent;
    


    void Update() {
        Vector3 centerPos = centerEyeTransform.position;
        Vector3 lookDirection = centerEyeTransform.rotation * Vector3.forward;
        cursor.transform.position = lookDirection * 3.0f + new Vector3(0.0f, 1.0f, 0.0f);

        bool timerSet = false;

        if (Vector3.Dot((nextButton.transform.position - centerPos).normalized, lookDirection) > cosineAngle) {
            Timer += Time.deltaTime;
            timerSet = true;

            if (Timer >= gazeTime)
            {
                nextButton.SetActive(false);
                gameMenu.SetActive(false);
                headTiltMenu.SetActive(true);
                startButton.SetActive(true);
                Timer = 0.0f;
            }
        }

        if (Vector3.Dot((startButton.transform.position - centerPos).normalized, lookDirection) > cosineAngle)
        {
            Timer += Time.deltaTime;
            timerSet = true;

            if (Timer >= gazeTime)
            {
                startButton.SetActive(false);
                headTiltMenu.SetActive(false);
                Timer = 0.0f;

                StartEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        if (!timerSet)
        {
            Timer = 0.0f;
        }
    }
}
