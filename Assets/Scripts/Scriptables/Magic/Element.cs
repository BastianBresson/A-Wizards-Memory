using UnityEngine;


[CreateAssetMenu( fileName = "Element", menuName = "Scriptables/Element" )]
public class Element : ScriptableObject
{
    public enum ElementEnum { Normal, Fire, Water, Earth };

    public ElementEnum ElementType;

    public GameObject ElementalSpellPrefab;
    public GameObject ElementalShieldPrefab;

    /// <summary>
    /// Checks if <c>b</c> is countered by <c>a</c>
    /// </summary>
    public bool Countered(ElementEnum a, ElementEnum b)
    {
        return (a == ElementEnum.Water   &&  b == ElementEnum.Fire ||
                a == ElementEnum.Earth  &&  b == ElementEnum.Water ||
                a == ElementEnum.Fire  &&  b == ElementEnum.Earth); 
    }
}
