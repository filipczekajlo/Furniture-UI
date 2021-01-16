using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private GameObject viewCanvas;
    [SerializeField] private GameObject allCanvas;
    [SerializeField] private State homeState;
    [SerializeField] private State productState;
    [SerializeField] private State formState;
    [SerializeField] private State complettionState;

    private void Awake()
    {
        Screen.fullScreen = false;
    }

    private void Start()
    {
        StateManager.viewCanvas = viewCanvas;
        StateManager.allCanvases = allCanvas;
        StateManager.homeState = homeState;
        StateManager.productState = productState;
        StateManager.formState = formState;
        StateManager.completionState = complettionState;
        homeState.gameObject.SetActive(true);
        homeState.transform.SetParent(viewCanvas.transform);
        homeState.transform.position = viewCanvas.transform.position;
    }
}
