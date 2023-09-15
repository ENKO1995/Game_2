using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AbilityInfo : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Sprite;
    public AbilityNames AbilityIndex;
}
