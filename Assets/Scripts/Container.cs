using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Container : MonoBehaviour, IContainer
{
    public List<IMorphState> elements = new List<IMorphState>();
    public GameObject invisibleTemplate;
    public DataSO data;
    public Container Reference { get; set; }
    public List<IMorphState> startElements = new List<IMorphState>();
    
    private IMorphState[] elementsInIcon;

    private void Awake()
    {
        AddElementsToList();
        startElements = GetElements();
        if (data != null)
        {
            InitializeData();
        }
    }

    private void InitializeData()
    {

            data.Initialize();
            var elements = GetComponentsInChildren<Element>();
            foreach ( var pair in data.keyVal)
            {
                foreach (var element in elements)
                {
                    if (element.Name == pair.Key)
                    {
                        element.GetComponent<TextMeshProUGUI>().text = pair.Value;
                    }
                }
            }
        
    }

    private List<IMorphState> GetElements()
    {
        var tempList = GetComponentsInChildren<IMorphState>().ToList();
        return tempList;
    }
    
    public void AddElementsToList()
    { 
        elements.Clear();
            elementsInIcon= GetComponentsInChildren<IMorphState>();
        foreach (var t in elementsInIcon)
        {
            elements.Add(t);
        }
    }
}
