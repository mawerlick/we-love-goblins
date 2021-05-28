using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelSnapper : MonoBehaviour
{
    [ContextMenu("Snap to Grid")]
    public void SnapToGrid()
    {
        Vector3 pos = gameObject.transform.position;
        float x = pos.x * 2, y = pos.y * 2, z = pos.z * 2;
        gameObject.transform.position = new Vector3(Mathf.Round(x)/2f, Mathf.Round(y)/2f, Mathf.Round(z)/2f);
    }
}
