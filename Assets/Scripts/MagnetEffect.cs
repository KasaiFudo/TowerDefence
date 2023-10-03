using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetEffect : MonoBehaviour
{
    [SerializeField]private float force = 1f;
    [SerializeField] private float radius = 20f;

    void FixedUpdate()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, radius))
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddForce((transform.position - collider.transform.position).normalized * force);
        }
    }
}
