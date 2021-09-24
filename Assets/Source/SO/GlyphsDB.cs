using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlyphsDB", menuName = "DB/Glyphs", order = 0)]
public class GlyphsDB : ScriptableObject
{
    [field: SerializeField]
    public Sprite[] BaseSprites { get; private set; }

    [field: SerializeField]
    public Sprite[] FinalSprites { get; private set; }

}
