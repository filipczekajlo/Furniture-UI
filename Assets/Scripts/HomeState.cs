using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HomeState : State
{
    private void Awake()
    {
        InstantiateContainerCopies();
        InitializeData();
    }

    public override void Show()
    {
        print("Set State to Homestate");
        foundElements.Clear();
        unmatchedElementsInGoal.Clear();
        unmatchedElementsInStart.Clear();
        SetTransform(gameObject, distanceFromViewCanvas);
        foundElements = GetMatchedElements(StateManager.container, StateManager.container.Reference);
        MoveElements(foundElements, moveDuration, ease);
        unmatchedElementsInStart = GetUnmatchedFromStart(StateManager.container, StateManager.container.Reference);
        unmatchedElementsInGoal = GetUnmatchedFromGoal(StateManager.container, StateManager.container.Reference);
        StartCoroutine(MoveandDestroy(unmatchedElementsInStart, StateManager.container.Reference, moveDuration, ease));
        ChangeParent(StateManager.container, StateManager.container.Reference.transform.parent);
        EnableAllElements(StateManager.container);
        StartCoroutine(FixPosition());
    }

    private IEnumerator FixPosition()
    {
        yield return new WaitForSeconds(moveDuration);
        MoveElements(foundElements, 0,ease);
    }
    private void EnableAllElements(Container container)
    {
        foreach (var element in container.startElements)
        {
            element.GameObject.SetActive(true);
        }
    }

    public override void Hide()
    {
        StartCoroutine(HideAfterTransition());
    }

    private IEnumerator HideAfterTransition()
    {
        yield return new WaitForSeconds(0.5f /*StateManager.currentState.moveDuration*/);
        UnsetTransform(gameObject);
    }

    private void InstantiateContainerCopies()
    {
        var tempArray = GetComponentsInChildren<Container>();
        var test = tempArray.ToList();
        foreach (var container in test)
        {
            var tempCopy = Instantiate(container, container.transform.position, Quaternion.identity, container.transform.parent);
            tempCopy.invisibleTemplate = GetComponentInChildren<CanvasGroup>().gameObject;
            container.Reference = tempCopy;
            tempCopy.Reference = container;
            container.gameObject.SetActive(false);
            container.Reference.gameObject.SetActive(true);
        }
    }
    private void InitializeData()
    {
        var tempConts = GetComponentsInChildren<Container>();
        print(tempConts.Count());
        for (var i = 0; i < tempConts.Length; i++)
        {
            if(tempConts[i].data == null)
                return;
            tempConts[i].data.Initialize();
            var elements = tempConts[i].GetComponentsInChildren<Element>();
            foreach ( var pair in tempConts[i].data.keyVal)
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
    }
}