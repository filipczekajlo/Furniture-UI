using UnityEngine;

public class ViewScreen : MonoBehaviour
{
    public Container cont;

    private void Update()
    {
        cont = StateManager.container;
    }
}
