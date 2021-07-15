using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatAmmo : MonoBehaviour
{
    public float angleModifier;
    public Rigidbody rb;
    public SphereCollider seedCollider;
    public Vector3 target;
    public GameObject splatExplosion;


    private void Start()
    {
        Vector3 vector = CalculateTrajectoryVelocity(transform.position, target, angleModifier);
        rb.velocity = vector;
    }

    Vector3 CalculateTrajectoryVelocity(Vector3 origin, Vector3 _target, float t)
    {
        float vx = (_target.x - origin.x) / t;
        float vz = (_target.z - origin.z) / t;
        float vy = ((_target.y - origin.y) - 0.5f * Physics.gravity.y * t * t) / t;
        return new Vector3(vx, vy, vz);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject splat = Instantiate(splatExplosion);
        splat.transform.position = transform.position;

        Destroy(gameObject);
    }
}
