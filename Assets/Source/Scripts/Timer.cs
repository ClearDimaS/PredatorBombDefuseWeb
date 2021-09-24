using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private int duration;
    [SerializeField] private Button addTimeBtn, removeTimeBtn;
    [SerializeField] private TMP_Text CurTimeText, TimeSpentText;

    float curTimeLeft;

    public bool IsRunning { get; private set; }

    public event Action OnTimerIsUp;

    private void Awake()
    {
        addTimeBtn.onClick.AddListener(AddTime);
        removeTimeBtn.onClick.AddListener(RemoveTime);
    }

    private void Start()
    {
        UpdateTime(duration);
        HideTimeSpent();
    }

    public void StartTimer(Action onTimerGoOut = null)
    {
        curTimeLeft = duration;
        OnTimerIsUp += onTimerGoOut;
        removeTimeBtn.gameObject.SetActive(false);
        addTimeBtn.gameObject.SetActive(false);
        StartCoroutine(TimerCoroutine());
        IsRunning = true;
        HideTimeSpent();
    }

    public void StopTimer()
    {
        removeTimeBtn.gameObject.SetActive(true);
        addTimeBtn.gameObject.SetActive(true);
        StopAllCoroutines();
        OnTimerIsUp = null;
        IsRunning = false;
        SetTimeSpent();
    }

    private void RemoveTime()
    {
        duration--;
        UpdateTime(duration);
    }

    private void AddTime()
    {
        duration++;
        UpdateTime(duration);
    }

    private void UpdateTime(float val)
    {
        CurTimeText.text = val.ToString();
    }

    IEnumerator TimerCoroutine()
    {
        while (curTimeLeft > 0)
        {
            curTimeLeft -= Time.deltaTime;
            yield return null;
            UpdateTime(curTimeLeft);
        }

        OnTimerIsUp?.Invoke();
        IsRunning = false;
        OnTimerIsUp = null;
    }

    private void SetTimeSpent()
    {
        TimeSpentText.gameObject.SetActive(true);
        TimeSpentText.text = (duration - curTimeLeft).ToString();
    }

    private void HideTimeSpent()
    {
        TimeSpentText.gameObject.SetActive(false);
    }
}
