using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void GoToProductViewState(Container movee)
    {
        StateManager.SetState(StateManager.productState, movee); 
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
