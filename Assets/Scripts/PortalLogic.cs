using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLogic : MonoBehaviour
{
    public GameObject OtherPortal;
    public float TeleportOffset;
    public bool active = true;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!active) return;
        var rigidBody = collider.GetComponent<Rigidbody2D>();

        var relPoint = transform.InverseTransformPoint(collider.transform.position);
        var relVelocity = -transform.InverseTransformDirection(rigidBody.velocity);
        rigidBody.velocity = OtherPortal.transform.TransformDirection(relVelocity);
        collider.transform.position = OtherPortal.transform.TransformPoint(relPoint) +
                                      (Vector3)rigidBody.velocity.normalized * TeleportOffset;
    }
}