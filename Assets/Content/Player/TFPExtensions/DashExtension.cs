using TheFirstPerson;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Dash movement TFP's extension
/// </summary>
public class DashExtension : TFPExtension
{
    private bool _firstButtonPressedA;
    private float _doubleClickOffsetA;
    private bool _resetA;
    
    private bool _firstButtonPressedD;
    private float _doubleClickOffsetD;
    private bool _resetD;
    
    
    public float dashStrenght = 20;

    public override void ExPreMove(ref TFPData data, TFPInfo info)
    {
        if (!data.crouching)
        {
            //A (Left)
            if(Input.GetKeyDown(KeyCode.A) && _firstButtonPressedA) {
                if(Time.time - _doubleClickOffsetA < 0.4f) {
                    Dash(ref data, info, Vector3.left);
                }
 
                _resetA = true;
            }
 
            if(Input.GetKeyDown(KeyCode.A) && !_firstButtonPressedA) {
                _firstButtonPressedA = true;
                _doubleClickOffsetA = Time.time;
            }
 
            if(_resetA) {
                _firstButtonPressedA = false;
                _resetA = false;
            }
            
            //D (Right)
            if(Input.GetKeyDown(KeyCode.D) && _firstButtonPressedD) {
                if(Time.time - _doubleClickOffsetD < 0.4f) {
                    Dash(ref data, info, Vector3.right);
                }
 
                _resetD = true;
            }
 
            if(Input.GetKeyDown(KeyCode.D) && !_firstButtonPressedD) {
                _firstButtonPressedD = true;
                _doubleClickOffsetD = Time.time;
            }
 
            if(_resetD) {
                _firstButtonPressedD = false;
                _resetD = false;
            }
        }
        
        base.ExPreMove(ref data, info);
    }

    private void Dash(ref TFPData data, TFPInfo info, Vector3 direction)
    {
        dashStrenght = 20;
        data.currentMove += this.transform.rotation * direction * dashStrenght;
    }
}
