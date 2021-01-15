
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ProductViewState : State
{
    [SerializeField] public Container templateContainer;

    
    private bool rotationEnabled = false;

    public override void Show()
    {
        Debug.Log("Set State to ProductViewState.");
        foundElements.Clear();
        unmatchedElementsInGoal.Clear();
        SetTransform(gameObject, distanceFromViewCanvas);
        EnableContainer(templateContainer, false);
        ChangeParent(StateManager.container, transform);
        foundElements = GetMatchedElements(StateManager.container, templateContainer);
        unmatchedElementsInGoal = GetUnmatchedFromGoal(StateManager.container, templateContainer);
        unmatchedElementsInStart = GetUnmatchedFromStart(StateManager.container, templateContainer);
        pairedUnmatchedGoalElements = SetupUnmatchedElementsCopy(unmatchedElementsInGoal, StateManager.container);
        EnableElements(unmatchedElementsInStart, false);
        MoveElements(foundElements, moveDuration, ease);
        MoveElements(pairedUnmatchedGoalElements, moveDuration, ease);

        SetupBody();
        SetupARButton();
        SeteupBuyButton();
    }

    private void SetupBody()
    {
        foreach (var element in pairedUnmatchedGoalElements)
        {
            foreach (var pair in StateManager.container.data.keyVal)
            {
                if (element.Key.Name == pair.Key)
                {
                    element.Key.GameObject.GetComponent<TextMeshProUGUI>().text = pair.Value;
                }
            }
        }
    }


    private void SetupARButton()
    {
        var button = GetElement(StateManager.container, "arbutton");
        if(button != null)
        State.AddEvent(button.GameObject, EventTriggerType.PointerClick, delegate {SwitchToARScene();});
    }

    private void SeteupBuyButton()
    {
        var button = GetElement(StateManager.container, "buybutton");
        if (button != null)
        {
            AddEvent(button.GameObject, EventTriggerType.PointerClick,delegate { StateManager.SetState(StateManager.formState);});
            return;
        }
        Debug.LogWarning("buybutton not found! You need to attach the Element script to the Container as a child and set the correct name!");
    }

    private void SwitchToARScene()
    {
        SceneManager.LoadScene(1);
    }
    
    public override void Hide()
    {
        UnsetTransform(gameObject);
    }
}
