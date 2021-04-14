using System;
using TMPro;
using UnityEngine;
public class Timer : MonoBehaviour
{
    [SerializeField] private float timeInMinutes;
    [SerializeField] private TMP_Text timerTextUI;
    [SerializeField] private GameObject houseInWayOfNextLevel;
    private bool needToChangeTime = true;

    private void Start()
    {
        timeInMinutes *= 60f;
    }

    private void Update()
    {
        if (timeInMinutes <= 0)
        {
            Destroy(houseInWayOfNextLevel);
            needToChangeTime = false;
            timerTextUI.text = GetTimeText(0);
        }
    }

    private void FixedUpdate()
    {
        if (needToChangeTime)
        {
            timeInMinutes -= Time.deltaTime;
            timerTextUI.text = GetTimeText(timeInMinutes);
        }
    }

    private String GetTimeText(float time)
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.Floor(time % 60);
        String minutesString, secondsString;
        if(minutes < 10) {
            minutesString = "0" + minutes.ToString();
        }
        else
        {
            minutesString = minutes.ToString();
        }
        if(seconds < 10) {
            secondsString = "0" + Mathf.RoundToInt(seconds).ToString();
        }
        else
        {
            secondsString = seconds.ToString();
        }

        return minutesString + ":" + secondsString;
    }
}
