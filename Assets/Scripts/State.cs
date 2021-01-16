using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class State : MonoBehaviour
{
    [Range(0f,5f)] 
    public float moveDuration = 1.0f;

    public Ease ease = Ease.Linear;
    
    public List<IMorphState> unmatchedInGoalCopy = new List<IMorphState>();
    protected Dictionary<IMorphState, IMorphState> foundElements = new Dictionary<IMorphState, IMorphState>();
    protected List<IMorphState> unmatchedElementsInStart = new List<IMorphState>();
    protected List<IMorphState> unmatchedElementsInGoal = new List<IMorphState>();
    protected Dictionary<IMorphState, IMorphState> pairedUnmatchedGoalElements = new Dictionary<IMorphState, IMorphState>();
    public float distanceFromViewCanvas;

    public abstract void Show();
    public abstract void Hide();

    protected static void ChangeParent(Container containerToAttach, Transform parent) 
    {
        containerToAttach.gameObject.SetActive(true);
        containerToAttach.gameObject.transform.SetParent(parent);
    }

    protected static void ChangeParent(List<IMorphState> elementsToAttach, Transform parent)
    {
        foreach (var element in elementsToAttach)
        {
            element.GameObject.transform.SetParent(parent);
        }
    }
    protected static void ChangeParent(IMorphState elementToAttach, Transform parent)
    {
        elementToAttach.GameObject.transform.SetParent(parent);
    }
     
    protected static void EnableContainer(Container templateContainer, bool value)
    {
        templateContainer.gameObject.SetActive(value);
    }
     
    protected static void SetTransform(GameObject stateGameObject, float distanceFromCanvas)
    {
        stateGameObject.transform.SetParent(StateManager.viewCanvas.transform);
        stateGameObject.transform.position = StateManager.viewCanvas.transform.position;
        stateGameObject.transform.position = new Vector3(stateGameObject.transform.position.x, stateGameObject.transform.position.y, distanceFromCanvas);
    }

    protected static void UnsetTransform(GameObject stateGameObject)
    {
        stateGameObject.transform.SetParent(StateManager.allCanvases.transform);
        stateGameObject.transform.position = StateManager.allCanvases.transform.position;
    }

    protected static Dictionary<IMorphState,IMorphState> GetMatchedElements(Container startContainer, Container goalContainer)
    {
        startContainer.AddElementsToList();
        goalContainer.AddElementsToList();
        Dictionary<IMorphState, IMorphState> tempDic = new Dictionary<IMorphState, IMorphState>();
        foreach (var goal in goalContainer.elements)
        {
            var match = startContainer.elements.Where(x => x.Name == goal.Name);
            foreach (var movee in match)
            {
                tempDic.Add(movee, goal);
            }
        }
        return tempDic;
    }

    protected static List<IMorphState> GetUnmatchedFromGoal(Container startContainer, Container goalContainer)
    {
        // Get Items from goalContainer that are not in startContainer.
        goalContainer.AddElementsToList();
        List<IMorphState> tempList = goalContainer.elements.Where(x => startContainer.elements.All(y => y.Name != x.Name)).ToList();
        return tempList;
    }

    protected static List<IMorphState> GetUnmatchedFromStart(Container startContainer, Container goalContainer)
    {
        // Get Items from startContainer that are not in goalContainer in order to deactivate them.
        startContainer.AddElementsToList();
        List<IMorphState> deactivateList = startContainer.elements.Where(x => goalContainer.elements.All(y => y.Name != x.Name)).ToList();
        return deactivateList;
    }

    protected static void EnableElements(List<IMorphState> elementsToSet, bool value)
    {
        foreach (var element in elementsToSet)
        {
            element.GameObject.SetActive(value);
        }
    }

    protected static List<IMorphState> CopyElements(List<IMorphState> elementsToCopy)
    {
        List<IMorphState> tempList = new List<IMorphState>();
        foreach (var element in elementsToCopy)
        {
            var obj = Instantiate(element.GameObject, element.GameObject.transform.parent);
            tempList.Add(obj.GetComponent<IMorphState>());
        }
        return tempList;
    }
    protected static List<IMorphState> CopyElements(List<IMorphState> elementsToCopy, Transform parent)
    {
        List<IMorphState> tempList = new List<IMorphState>();
        foreach (var element in elementsToCopy)
        {
            var obj = Instantiate(element.GameObject, parent);
            tempList.Add(obj.GetComponent<IMorphState>());
        }
        return tempList;
    }
    protected static Dictionary<IMorphState, IMorphState> SetupUnmatchedElementsCopy(List<IMorphState> unfoundElements, Container container)
    {
        Dictionary<IMorphState, IMorphState> tempDic = new Dictionary<IMorphState, IMorphState>();

        foreach (var element in unfoundElements)
        {
            var elementCopy = Instantiate(element.GameObject, container.transform.position, Quaternion.identity, container.transform);
            elementCopy.transform.localScale = container.transform.lossyScale;
            tempDic.Add(elementCopy.GetComponent<IMorphState>(), element);
        }
        foreach (var element in tempDic) 
        {
            var text = element.Key.GameObject.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.alpha = 0f;
            }
            var button = element.Key.GameObject.GetComponent<Button>();
            if (button != null && element.Key.GameObject.GetComponent<IMorphState>().Name == "close")
            {
                AddEvent(button.gameObject, EventTriggerType.PointerClick, delegate { StateManager.SetState(StateManager.homeState); });
            }
        }

        return tempDic;
    }
    protected static void MoveElements(Dictionary<IMorphState,IMorphState> keyValuePair, float duration, Ease ease) // To Reveal
    {
        foreach (var pair in keyValuePair)
            {
                pair.Key.MorphTo(pair.Value.GameObject, duration, ease);
            }
    }

    protected static IEnumerator MoveandDestroy(List<IMorphState> elementsToMove, Container goalContainer, float duration, Ease ease) // To Hide
    {
        foreach (var element in elementsToMove)
        {
            element.MorphTo(goalContainer.invisibleTemplate, duration, ease);
        }
        yield return new WaitForSeconds(duration);
        foreach (var element in elementsToMove)
        {
            Destroy(element.GameObject);
        }
    }

    protected static IEnumerator DestroyAfter(Container container, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(container.gameObject);
    }
    public static IMorphState GetElement(Container container, string name)
    {
        var tempElements = container.GetComponentsInChildren<Element>();
        foreach (var element in tempElements)
        {
            if (element.Name == name)
            {
                return element;
            }
        }
        return null;
    }

    public static IMorphState GetElement(List<IMorphState> elementList, string name)
    {
        foreach (var element in elementList)
        {
            if (element.Name == name)
            {
                return element;
            }
        }
        return null;
    }

    public static void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry {eventID = type}; //.Entry because it's an entry to the EventTriggers
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    
}
