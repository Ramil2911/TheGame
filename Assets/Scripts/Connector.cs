using UnityEngine;

public class Connector : MonoBehaviour
{
    public Vector2 size = Vector2.one * 4.0f;
    public bool isConnected;

    private bool _isPlaying = false;

    void Start()
    {
        _isPlaying = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isConnected ? Color.green : Color.red;
        if (!_isPlaying)
        {
            Gizmos.color = Color.cyan;
        }
        
        Vector2 halfSize = size * 0.5f;
        Vector3 offset = transform.position + transform.up * halfSize.y;
        Gizmos.DrawLine(offset, offset + transform.forward);
        // define top and side vectors
        Vector3 top = transform.up * size.y;
        Vector3 side = transform.right * halfSize.x;
        //define corner vectors
        Vector3 topRight = transform.position + top + side;
        Vector3 topLeft = transform.position + top - side;
        Vector3 bottomRight = transform.position + side;
        Vector3 bottomLeft = transform.position - side;
        
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        //draw diagonal lines
        Gizmos.color *= 0.7f;
        Gizmos.DrawLine(topRight, bottomLeft);
        Gizmos.DrawLine(topLeft, bottomRight);
    }
}
