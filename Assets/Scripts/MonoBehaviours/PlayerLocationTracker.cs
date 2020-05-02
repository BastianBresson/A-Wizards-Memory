using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocationTracker : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Collider myCollider = this.gameObject.GetComponent<Collider>();

            Vector3 position = this.transform.position;
            float yExtent = myCollider.bounds.extents.y;

            position.y = (2 * yExtent) + 10;

            GameManager.Instance.FallRespawnPosition = position;
        }
    }
}
