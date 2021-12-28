using TheFirstPerson;
using UnityEngine;

public class LegsIKExtension : TFPExtension
{
    public GameObject leftLegTarget;
    public GameObject rightLegTarget;
    public GameObject rig;

    public float moveOffset = 0.8f;

    private Vector3 _leftLegStart;
    private Vector3 _rightLegStart;

    private void Start()
    {
        _leftLegStart = leftLegTarget.transform.localPosition;
        _rightLegStart = rightLegTarget.transform.localPosition;
    }

    public override void ExPostMove(ref TFPData data, TFPInfo info)
    {
        leftLegTarget.transform.localPosition -= transform.InverseTransformPoint(new Vector3(data.moveDelta.x, 0, data.moveDelta.z));
        rightLegTarget.transform.localPosition -= transform.InverseTransformPoint(new Vector3(data.moveDelta.x, 0, data.moveDelta.z));
        
        leftLegTarget.transform.rotation = 
        rightLegTarget.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);

        if ((leftLegTarget.transform.localPosition - _leftLegStart).magnitude > moveOffset)
        {
            leftLegTarget.transform.localPosition =
                -(leftLegTarget.transform.localPosition - _leftLegStart)*0.9f + _leftLegStart;
        }
        if ((rightLegTarget.transform.localPosition - _rightLegStart).magnitude > moveOffset)
        {
            rightLegTarget.transform.localPosition =
                -(rightLegTarget.transform.localPosition - _rightLegStart)*0.9f + _rightLegStart;
        }
        
        base.ExPostMove(ref data, info);
    }
}
