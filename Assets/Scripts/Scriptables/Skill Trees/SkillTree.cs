using System;
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


    public Tuple<int, int> ElementLevels(Element element)
    {
        int projectilesLvl = 0;
        int projectileMultiplierLvl = 0;

        switch (element.ElementType)
        {
            case Element.ElementEnum.Normal:
                break;
            case Element.ElementEnum.Fire:
                projectilesLvl = FireMultiplerLvl;
                projectileMultiplierLvl = FireMultiplerLvl;
                break;
            case Element.ElementEnum.Water:
                projectilesLvl = WaterProjectilesLvl;
                projectileMultiplierLvl = WaterMultiplierLvl;
                break;
            case Element.ElementEnum.Earth:
                projectilesLvl = EarthProjectilesLvl;
                projectileMultiplierLvl = EarthMultiplierLvl;
                break;
            default:
                projectilesLvl = 0;
                projectileMultiplierLvl = 0;
                break;
        }

        return new Tuple<int, int>(projectilesLvl, projectileMultiplierLvl);
    }


    public void UpgradeSkillTree(Element element, UpgradeType upgradeType)
    {
        switch (element.ElementType)
        {
            case Element.ElementEnum.Normal:
                break;
            case Element.ElementEnum.Fire:
                if (upgradeType == UpgradeType.Projectiles) { FireProjectilesLvl++; }
                else { FireMultiplerLvl++; }
                break;
            case Element.ElementEnum.Water:
                if (upgradeType == UpgradeType.Projectiles) { WaterProjectilesLvl++; }
                else { WaterMultiplierLvl++; }
                break;
            case Element.ElementEnum.Earth:
                if (upgradeType == UpgradeType.Projectiles) { EarthProjectilesLvl++; }
                else { EarthMultiplierLvl++; }
                break;
            default:
                break;
        }
    }


    public void ClearTree()
    {
        EarthProjectilesLvl = 0;
        EarthMultiplierLvl = 0;

        FireProjectilesLvl = 0;
        FireMultiplerLvl = 0;

        WaterProjectilesLvl = 0;
        WaterMultiplierLvl = 0;
    }

}
