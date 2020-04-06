using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBehaviour : MonoBehaviour
{
    public delegate void SelectMethod();

    /// <summary>
    /// The method that is called on selection
    /// </summary>
    public SelectMethod OnSelect;

    /// <summary>
    /// Notifies that the selector has been selected
    /// </summary>
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

    /// <summary>
    /// Notifies the player controller if it can select.
    /// </summary>
    /// <param name="entered">True if player is in range and can select, false if not </param>
    /// <param name="player">The collided PlayerController</param>
    public void NotifyPlayerController(bool entered, PlayerController player)
    {
        player.SelectCollision(entered, this);
    }
}
