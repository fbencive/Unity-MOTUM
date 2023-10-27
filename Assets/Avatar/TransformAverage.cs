using UnityEngine;

public class TransformAverage
{
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 cumulativePosition = new Vector3();
    private Vector4 cumulativeRotation = new Vector4();
    private Vector3 finalPosition = new Vector3();
    private Quaternion finalRotation = new Quaternion();
    private Quaternion firstRotation;
    private int addAmount = 0;

    public Vector3 position
    {
        get
        {
            return finalPosition;
        }
    }

    public Quaternion rotation
    {
        get
        {
            return finalRotation;
        }
    }

    public TransformAverage(Vector3 pos, Quaternion rot)
    {
        startingPosition = pos;
        startingRotation = rot;
    }

    public void Add(Vector3 newPosition, Quaternion newRotation)
    {
        addAmount++;
        cumulativePosition += newPosition;
        finalPosition = startingPosition - cumulativePosition / addAmount;

        if (addAmount == 1)
            firstRotation = newRotation;
        float w = 0.0f;
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        //Before we add the new rotation to the average (mean), we have to check whether the quaternion has to be inverted. Because
        //q and -q are the same rotation, but cannot be averaged, we have to make sure they are all the same.
        if (!AreQuaternionsClose(newRotation, firstRotation))
        {
            newRotation = InverseSignQuaternion(newRotation);
        }

        //Average the values
        float addDet = 1f / (float)addAmount;
        cumulativeRotation.w += newRotation.w;
        w = cumulativeRotation.w * addDet;
        cumulativeRotation.x += newRotation.x;
        x = cumulativeRotation.x * addDet;
        cumulativeRotation.y += newRotation.y;
        y = cumulativeRotation.y * addDet;
        cumulativeRotation.z += newRotation.z;
        z = cumulativeRotation.z * addDet;

        //note: if speed is an issue, you can skip the normalization step
        finalRotation = startingRotation * Quaternion.Inverse(NormalizeQuaternion(x, y, z, w));
    }

    Quaternion NormalizeQuaternion(float x, float y, float z, float w)
    {

        float lengthD = 1.0f / (w * w + x * x + y * y + z * z);
        w *= lengthD;
        x *= lengthD;
        y *= lengthD;
        z *= lengthD;

        return new Quaternion(x, y, z, w);
    }

    //Changes the sign of the quaternion components. This is not the same as the inverse.
    Quaternion InverseSignQuaternion(Quaternion q)
    {

        return new Quaternion(-q.x, -q.y, -q.z, -q.w);
    }

    //Returns true if the two input quaternions are close to each other. This can
    //be used to check whether or not one of two quaternions which are supposed to
    //be very similar but has its component signs reversed (q has the same rotation as
    //-q)
    bool AreQuaternionsClose(Quaternion q1, Quaternion q2)
    {

        float dot = Quaternion.Dot(q1, q2);

        if (dot < 0.0f)
        {

            return false;
        }

        else
        {

            return true;
        }
    }
}