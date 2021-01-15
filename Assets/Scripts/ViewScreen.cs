using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewScreen : MonoBehaviour
{
    public Container cont;

    private void Update()
    {
        cont = StateManager.container;
    }
}
