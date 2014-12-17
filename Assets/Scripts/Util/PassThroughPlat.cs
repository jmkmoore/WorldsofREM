using UnityEngine;
using System.Collections;

public class PassThroughPlat : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(transform.collider2D.gameObject.layer + " collided with " + other.collider2D.gameObject.layer);
        if (!transform.parent.GetComponent<Player>().GetGrounded() && other.collider2D.gameObject.layer == LayerMask.NameToLayer("OnewayPass"))
        {
           Physics2D.IgnoreLayerCollision(9, 8, true);
        }
    }
}
