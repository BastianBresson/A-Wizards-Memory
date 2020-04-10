using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType { None, Multiplier, Projectiles };

[CreateAssetMenu(fileName = "LevelSelect", menuName = "Scriptables/Level Select")]
public class LevelSelect : ScriptableObject
{
    public Element element;
    public UpgradeType upgradeType;
    public Material material;
}
