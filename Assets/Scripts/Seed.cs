using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public StrainProfile currentStrain;
    public string strainID;

    public float angleModifier;
    public Rigidbody rb;
    public SphereCollider collider;
    public Transform target;


    private void Start()
    {
        Vector3 vector = CalculateTrajectoryVelocity(transform.position, target.transform.position, angleModifier);
        rb.velocity = vector;

        strainID = currentStrain.strainID;
    }

    Vector3 CalculateTrajectoryVelocity(Vector3 origin, Vector3 target, float t)
    {
        float vx = (target.x - origin.x) / t;
        float vz = (target.z - origin.z) / t;
        float vy = ((target.y - origin.y) - 0.5f * Physics.gravity.y * t * t) / t;
        return new Vector3(vx, vy, vz);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform != target)
        {
            Physics.IgnoreCollision(collision.collider, collider);
        }
        else
        {
            print("TARGET IT!");
        }
    }

}
