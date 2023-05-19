using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GearRaycaster : MonoBehaviour
{
    public bool IsDragging { get { return _isDragging; } }
    [SerializeField] LayerMask _moveLayer, _hitLayer, _startLayer;
    [SerializeField] GameObject _prefab, _targetObject, _fX, _objectToDisable;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _smoothingFactor = 20f;
    Transform _dragTransform = null;
    bool _isDragging, _isInDragArea;
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
            StartRaycast(ray);

        else if (Input.GetMouseButtonUp(0) && _isDragging)
            DropRaycast(ray);

        else if (_isDragging)
            Drag(ray);
    }

    private void DropRaycast(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _hitLayer))
        {
            _targetObject.SetActive(true);
            Tween();

            if (hit.transform.gameObject.name == "child")
                hit.transform.parent.gameObject.SetActive(false);

            hit.transform.gameObject.SetActive(false);

            if (_objectToDisable != null)
                _objectToDisable.SetActive(false);

            _fX.SetActive(true);
            _text.text = "0X";

            Destroy(_dragTransform.gameObject);
            _isDragging = false;
        }
    }

    private void Drag(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _moveLayer))
        {
            _dragTransform.position = Vector3.Lerp(_dragTransform.transform.position, hit.point, Time.deltaTime * _smoothingFactor);
        }
    }

    private void StartRaycast(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _startLayer))
        {
            _dragTransform = GameObject.Instantiate(_prefab, _prefab.transform.position, _prefab.transform.localRotation).transform;
            _isDragging = true;
        }
    }

    private void Tween()
    {
        Vector3 baseScale = _targetObject.transform.localScale;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_targetObject.transform.DOScale(baseScale * 1.25f, .25f)).
        Append(_targetObject.transform.DOScale(baseScale, .25f));
    }
}
