using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillTree", menuName = "Scriptables/Skill Tree")]
public class SkillTree : ScriptableObject
{
    public int EarthProjectilesLvl = 0, FireProjectilesLvl = 0, WaterProjectilesLvl = 0;
    public int EarthMultiplierLvl = 0, FireMultiplerLvl = 0, WaterMultiplierLvl = 0;
    public float MultiplierValue = 0.25f;

    public int Level = 1;
    public Element[] AvailableElements;

    // TODO: Implement player save and load.
}
