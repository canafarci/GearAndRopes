using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dreamteck.Splines;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RopeLinearDragRaycaster : MonoBehaviour
{
    public bool IsDragging { get { return _isDragging; } }
    [SerializeField] LayerMask _moveLayer, _hitLayer, _startLayer;
    [SerializeField] Rope[] _ropes;
    [SerializeField] GameObject _endFX, _dragRope;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _smoothingFactor = 20f;
    Transform _dragTransform = null;
    bool _isDragging, _isInDragArea;
    GameObject _dragObject;
    int _currentIndex = 0;

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
        GameObject target = _ropes[_currentIndex].Target;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, Mathf.Infinity, _hitLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.transform.gameObject == target)
            {
                DropBehaviour(target, hit);
                return;
            }
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
            _dragTransform = GameObject.Instantiate(_dragRope, hit.point, _dragRope.transform.rotation).GetComponent<DragRope>().End;
            _dragObject = _dragTransform.parent.gameObject;
            _isDragging = true;
        }
    }

    private void DropBehaviour(GameObject target, RaycastHit hit)
    {
        GameObject rope = GameObject.Instantiate(_ropes[_currentIndex].Prefab, target.transform.position, _ropes[_currentIndex].Prefab.transform.rotation, transform);
        rope.transform.localRotation = _ropes[_currentIndex].Prefab.transform.rotation;

        Renderer[] renderers = rope.GetComponentsInChildren<Renderer>();
        renderers.ToList().ForEach(x => x.material.SetFloat("_speed", 2.5f));

        hit.transform.gameObject.SetActive(false);

        Sequence sequence = DOTween.Sequence();

        _ropes[_currentIndex].Gears.ToList().ForEach(x =>
        {
            x.IsActive = true;
            Vector3 baseScale = x.transform.localScale;
            sequence.Append(x.transform.DOScale(baseScale * 1.2f, 0.15f));
            sequence.Append(x.transform.DOScale(baseScale, 0.15f));
        });

        //fx
        _ropes[_currentIndex].FXsToActivate.ToList().ForEach(x => x.SetActive(true));
        _ropes[_currentIndex].FXsToDeactivate.ToList().ForEach(x => x.SetActive(false));

        if (_currentIndex == 2)
            StartCoroutine(DelayedEnable());

        _currentIndex++;

        _text.text = (3 - _currentIndex).ToString() + "X";
        Tween();

        Destroy(_dragObject);
        _isDragging = false;
    }

    IEnumerator DelayedEnable()
    {
        yield return new WaitForSeconds(.75f);
        _endFX.SetActive(true);
        yield return new WaitForSeconds(8f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Tween()
    {
        Vector3 baseScale = _dragTransform.transform.localScale;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_dragTransform.transform.DOScale(baseScale * 1.25f, .25f)).
        Append(_dragTransform.transform.DOScale(baseScale, .25f));
    }
}
