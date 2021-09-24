using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlyphChanger : MonoBehaviour
{
    [SerializeField] private Image crntIcon, desiredIcon;

    public Sprite[] _sprites;
    public int[] _substituteIndexes;
    public int curInd, _desiredSubIndex;

    public event Action Fail, Success;

    public void SetGlyphs(Sprite[] sprites, int[] substituteIndexes, int desiredSubIndex)
    {
        Debug.Assert(sprites.Length == substituteIndexes.Length);

        _sprites = sprites;
        _substituteIndexes = substituteIndexes;
        _desiredSubIndex = desiredSubIndex;
        Debug.Log($"{ _desiredSubIndex } - is desired index");
        curInd = 0;
        UpdateGlyphDisp(_substituteIndexes[0]);
    }

    public void Activate()
    {
        desiredIcon.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        desiredIcon.gameObject.SetActive(false);
    }

    public void TryAccept()
    {
        string arrStr = "";
        foreach (var item in _substituteIndexes)
        {
            arrStr += $"{item}, ";
        }
        Debug.Log($"{GetCurSubIndex() == _desiredSubIndex}" +
            $"{GetCurSubIndex()}  ==   {_desiredSubIndex}, cur rela ind: {curInd},   {arrStr}     {name}");
        if (GetCurSubIndex() == _desiredSubIndex)
            Success?.Invoke();
        else
            Fail?.Invoke();
    }

    public void Next()
    {
        ++curInd;
        UpdateGlyphDisp(GetCurSubIndex());
    }

    public void Prev()
    {
        --curInd;
        UpdateGlyphDisp(GetCurSubIndex());
    }

    private int GetCurSubIndex()
    {
        int cuIndAbs = Mathf.Abs(curInd);
        return _substituteIndexes[cuIndAbs % _substituteIndexes.Length];
    }

    private void UpdateGlyphDisp(int curSubstituteIndex)
    {
        crntIcon.sprite = _sprites[curSubstituteIndex];
        desiredIcon.sprite = _sprites[_desiredSubIndex];
        Debug.Log(curSubstituteIndex == _desiredSubIndex);
    }
}
