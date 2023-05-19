using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeColorMaster : MonoBehaviour
{
    [SerializeField] public bool FirstCark, FirstVida, FirstUnderbase;

    [SerializeField] public Color _cark1, _cark2, _vida1, _vida2, _underbase1, _underbase2;

    private void Start() {
        Invoke("Change", 0.2f);
    }

    private void Change() {
        {
            FindObjectsOfType<ChangeColor>().ToList().ForEach(x => x.ChangeItemColor( FirstUnderbase ? _underbase1 : _underbase2,  FirstVida ? _vida1 : _vida2 ));
        }
    }
}
