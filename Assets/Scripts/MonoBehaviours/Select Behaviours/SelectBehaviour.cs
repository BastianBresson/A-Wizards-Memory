using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBehaviour : MonoBehaviour
{
    public delegate void SelectMethod();

    public SelectMethod OnSelect;


    public void Selected()
    {
        if (OnSelect != null)
        {
            OnSelect();
        }
        else
        {
            Debug.LogError("The OnSelect method has not been set by the controlling GameObject");
        }
    }


    public void NotifyPlayerController(bool entered, PlayerController player)
    {
        player.SelectCollision(entered, this);
    }
}
