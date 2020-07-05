using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpell", menuName = "Scriptables/Elemental Projectile")]
public class ProjectileSpell : Spell
{
    public Element Element;
    public float Speed;

    public GameObject ExplosionParticle;

    public string ChargeAudioName()
    {
        switch (Element.ElementType)
        {
            case Element.ElementEnum.Normal:
                break;
            case Element.ElementEnum.Fire:
                return "FireCharge";
            case Element.ElementEnum.Water:
                return "WaterCharge";
            case Element.ElementEnum.Earth:
                return "EarthCharge";
            default:
                break;
        }
        return string.Empty;
    }

    public string LaunchAudioName()
    {
        switch (Element.ElementType)
        {
            case Element.ElementEnum.Normal:
                break;
            case Element.ElementEnum.Fire:
                return "FireLaunch";
            case Element.ElementEnum.Water:
                return "WaterLaunch";
            case Element.ElementEnum.Earth:
                return "EarthLaunch";
            default:
                break;
        }
        return string.Empty;
    }
}
