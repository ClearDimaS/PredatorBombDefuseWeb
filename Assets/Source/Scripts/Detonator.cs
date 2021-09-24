using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

public class Detonator : MonoBehaviour
{
    [SerializeField] private GlyphsDB glyphsDB;
    [SerializeField] private GlyphChanger[] glyphChangers;
    [SerializeField] private Timer timer;
    [SerializeField] private GameObject[] activeInGameGOs;

    int completedGlyphs;

    void Start()
    {
        ResetGlyphs();

        ConnectEvents();

        SetInGameObjects(false);
    }

    public void Launch()
    {
        ResetGlyphs();
        SetGlyphsData();

        glyphChangers[0].Activate();
        timer.StartTimer(Lose);
        SetInGameObjects(true);
    }

    private void SetGlyphsData(int start = 0, int end = -1)
    {
        if (end == -1)
            end = glyphChangers.Length - 1;

        for (int i = start; i < end; i++)
        {
            SetGlyph(glyphChangers[i], glyphsDB.BaseSprites);
        }

        if (end < glyphChangers.Length - 1)
            return;

        SetGlyph(glyphChangers[glyphChangers.Length - 1], glyphsDB.FinalSprites);
    }

    private void SetGlyph(GlyphChanger glyphChanger, Sprite[] sprites)
    {
        GetRandomIndexes(sprites.Length, out int[] substituteIndexes, out int randomSubstituteIndex);
        glyphChanger.SetGlyphs(sprites, substituteIndexes, randomSubstituteIndex);
    }

    private void GetRandomIndexes(int end, out int[] substituteIndexes, out int randomSubstituteIndex)
    {
        List<int> inOrder = new List<int>(end);
        for(int i = 0; i < end; i++)
            inOrder.Add(i);

        System.Random rand = new System.Random();
        substituteIndexes = inOrder.OrderBy(x => rand.NextDouble()).ToArray();
        randomSubstituteIndex = (int)(rand.NextDouble() * end);
    }

    private void ConnectEvents()
    {
        for (int i = 0; i < glyphChangers.Length; i++)
        {
            glyphChangers[i].Fail += RollBack;
            glyphChangers[i].Success += GoNext;
        }
    }

    private void RollBack()
    {
        glyphChangers[completedGlyphs].Deactivate();

        if (completedGlyphs - 1 >= 0)
            --completedGlyphs;

        SetGlyphsData(completedGlyphs, completedGlyphs+1);
        glyphChangers[completedGlyphs].Activate();
    }

    private void GoNext()
    {
        glyphChangers[completedGlyphs].Deactivate();

        if (completedGlyphs < glyphChangers.Length - 1)
            ++completedGlyphs;
        else
        {
            Win();
            return;
        }

        SetGlyphsData(completedGlyphs, completedGlyphs + 1);
        glyphChangers[completedGlyphs].Activate();
    }

    private void ResetGlyphs()
    {
        foreach (var item in glyphChangers)
        {
            item.Deactivate();
        }
    }

    public void TryAcceptCurrent()
    {
        if(timer.IsRunning)
            glyphChangers[completedGlyphs].TryAccept();
    }

    public void CurrentGlyphIncrValue()
    {
        if (timer.IsRunning)
            glyphChangers[completedGlyphs].Next();
    }

    public void CurrentGlyphDecrValue()
    {
        if (timer.IsRunning)
            glyphChangers[completedGlyphs].Prev();
    }

    private void SetInGameObjects(bool active)
    {
        foreach (var item in activeInGameGOs)
        {
            item.SetActive(active);
        }
    }

    private void Win()
    {
        timer.StopTimer();
        SetInGameObjects(false);
    }

    private void Lose()
    {
        ResetGlyphs();
        timer.StopTimer();
        SetInGameObjects(false);
    }
}
