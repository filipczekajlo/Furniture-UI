using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

[CreateAssetMenu]
public class DataSO : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private string designer;
    [SerializeField] private string price;
    [TextArea(10, 100)]
    [SerializeField] private string body;

    public Dictionary<string, string> keyVal = new Dictionary<string, string>();

    public void Initialize()
    {
        keyVal.Clear();
        keyVal.Add("name", name);
        keyVal.Add("designer", designer);
        keyVal.Add("price", price);
        keyVal.Add("body", body);
    }
}
