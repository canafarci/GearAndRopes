using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public void ChangeItemColor(Color color, Color color2)
    {
        Renderer renderer = GetComponent<Renderer>();
            if (renderer == null) return;
        renderer.materials[0].DOColor(color, .1f);
            renderer.materials[1].DOColor(color2, .1f);
    }
}
