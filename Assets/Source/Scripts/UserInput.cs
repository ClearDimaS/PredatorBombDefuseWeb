using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserInput : MonoBehaviour
{
    [SerializeField] Detonator detonator;
    [SerializeField] Button goBtn, acceptBtn, nextBtn, prevBtn;
    [SerializeField] EventSystem eventSystem;

    private void Awake()
    {
        goBtn.onClick.AddListener(() => detonator.Launch());
        acceptBtn.onClick.AddListener(() => detonator.TryAcceptCurrent());
        nextBtn.onClick.AddListener(() => detonator.CurrentGlyphIncrValue());
        prevBtn.onClick.AddListener(() => detonator.CurrentGlyphDecrValue());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            detonator.CurrentGlyphIncrValue();
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            detonator.CurrentGlyphDecrValue();
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            detonator.TryAcceptCurrent();

        eventSystem.SetSelectedGameObject(null);
    }
}
