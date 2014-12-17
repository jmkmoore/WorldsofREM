using UnityEngine;
using System.Collections;

public class PassThroughPlat : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!transform.parent.GetComponent<Player>().GetGrounded() && other.collider.gameObject.layer == LayerMask.NameToLayer("OnewayPass"))
        {
           Physics.IgnoreLayerCollision(9, 8, true);
        }
    }
}
