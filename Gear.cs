using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Gear : MonoBehaviour
{
    public bool IsActive, IsReverse = false;
    float _baseSpeed = 5f;
    float _baseRadius = 1f;
    float _thisSpeed, _thisRadius;

    private void Start()
    {
        ChangeColorMaster _master = FindObjectOfType<ChangeColorMaster>();
        bool isFirst = _master.FirstCark;
        Color color = isFirst ? _master._cark1 : _master._cark2;

        GetComponent<Renderer>().material.DOColor(color, .1f);

        MeshCollider collider = GetComponent<MeshCollider>();

        if (collider == null) return;
        
        Mesh mesh = collider.sharedMesh;

        Vector3 furthestVector = mesh.vertices.ToList().OrderBy(x => Vector3.Distance(x, transform.position)).First();
        Vector3 distantVector = mesh.vertices.OrderBy(x => Vector3.Distance(x, furthestVector)).Last();
        _thisRadius = Vector3.Distance(furthestVector, distantVector);

        _thisSpeed = (_baseRadius * _baseSpeed) / _thisRadius;


        

        
    }

    private void Update()
    {
        if (!IsActive) { return; }

        Vector3 rot = transform.localRotation.eulerAngles;

        if (IsReverse)
            rot.z += _thisSpeed * Time.deltaTime;
        else
            rot.z -= _thisSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rot);
    }
}
