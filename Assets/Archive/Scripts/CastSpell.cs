using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CastSpell : ScriptableObject
{
    public abstract void Cast(GameObject caster, GameObject spell, Vector3 rayHitLocation);
}

