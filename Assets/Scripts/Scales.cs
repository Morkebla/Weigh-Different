using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scales : MonoBehaviour
{
    public Rigidbody2D beam;
    public ScalesPan leftPan;
    public ScalesPan rightPan;
    
    void Update()
    {
        float targetBeamRotation = 0;

        if (leftPan.TotalMass != rightPan.TotalMass)
        {
            float rotation = leftPan.TotalMass - rightPan.TotalMass;
            rotation = Mathf.Clamp(rotation, -30, 30);
            float sign = Mathf.Sign(rotation);
            rotation = Mathf.Pow(Mathf.Abs(rotation), 0.1f) * sign * 10;

            targetBeamRotation = rotation;
        }

        beam.rotation = Mathf.Lerp(beam.rotation, targetBeamRotation, Time.deltaTime);
    }
}
