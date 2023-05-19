using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRope : MonoBehaviour
{
    [SerializeField] bool _adjustRotation;
    [SerializeField] Vector3 _rotation;
    public Transform End;

    private void Start()
    {
        if (_adjustRotation)
        {
            Invoke("SetRotation", 0.1f);
        }
    }

    void SetRotation()
    {
        End.localRotation = Quaternion.Euler(_rotation);
    }
}
