using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    [SerializeField] private IMorphState[] elements;
    private List <IMorphState> threeModels = new List<IMorphState>();
    [SerializeField] private Transform scrollPosition;

    [SerializeField] private float rotationOffset;
    
    private void Start()
    {
        elements = GetComponentsInChildren<IMorphState>();
        foreach (var elem in elements)
        {
            threeModels = elements.Where(x => x.Name == "3d").ToList();
        }
        AdjustRotation();

    }

    public void OnScroll()
    {
        AdjustRotation();
    }

    private void AdjustRotation()
    {
        foreach (var element in threeModels)
        {
            var position = element.GameObject.transform.position.y;
            if (position > 0)
            {
                var newval = ConvertRange(0, 70, 0, rotationOffset, position);

                element.GameObject.transform.eulerAngles = new Vector3(newval,element.GameObject.transform.eulerAngles.y,element.GameObject.transform.eulerAngles.z); 
            }
        }
    }

    static float ConvertRange(
        float originalStart, float originalEnd, // original range
        float newStart, float newEnd, // desired range
        float value) // value to convert
    {
        double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
        return (float)(newStart + ((value - originalStart) * scale));
    }
}
