using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class StateManager 
{
    public static State currentState;
    public static State previousState;
    
    public static GameObject viewCanvas;
    public static GameObject allCanvases;
    public static State homeState;
    public static State productState;
    public static State formState;
    public static State completionState;

    public static Container container;
    public static void Initialize(State state)
    {
        currentState = state;
        Debug.Log("Set " + state + "as starting State!");
    }

    public static void SetState(State state, Container passedContainer)
    {
        previousState = currentState;
        currentState = state;
        container = passedContainer;
        currentState.gameObject.SetActive(true);
        currentState.Show();
        if (previousState != null)
        {
            previousState.Hide();
        }
    }
    public static void SetState(State state)
    {
        previousState = currentState;
        currentState = state;
        currentState.gameObject.SetActive(true);
        currentState.Show();
        previousState.Hide();
    }

}
