using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;

public class CompletionState : State
{
    [SerializeField] private float congratsDelay;
    [SerializeField] private float loadHomeAfter = 1;
    [Space]
    [SerializeField] private float hideElementsDuration = 0.4f;
    [SerializeField] private Ease hideElementsEase;
    [SerializeField] private Container templateContainer;
    [Space]
    
    [SerializeField] private Transform vanFirstCheckpoint;
    [SerializeField] private float vanMoveInDuration;
    [SerializeField] private Ease vanMoveInEase;
    [SerializeField] private Transform vanSecondCheckpoint;
    [SerializeField] private float vanSecondDuration;
    [SerializeField] private Ease vanSecondEase;
    [Space]
    [SerializeField] private Transform furnitureFirstCheckpoint;
    [SerializeField] private float furnitureFirstMoveDuration;
    [SerializeField] private Ease furnitureFirstEase;
    [Space] 
    [SerializeField] private Transform furnitureSecondCheckPoint;
    [SerializeField] private float furnitureSecondMoveDuration;
    [SerializeField] private Ease furnitureSecondEase;
    [Space]
    [SerializeField] private Transform formFirstCheckpoint;
    [SerializeField] private float formFirstCheckDuration;
    [SerializeField] private Ease formFirstEase;


    public override void Show()
    {
        foundElements.Clear();
        unmatchedElementsInGoal.Clear();
        SetTransform(gameObject, distanceFromViewCanvas);
        ChangeParent(StateManager.container, transform);
        var clip = GetElement(StateManager.previousState.unmatchedInGoalCopy, "form");
        ChangeParent(clip, StateManager.container.transform);
        EnableContainer(templateContainer, false);
        foundElements = GetMatchedElements(StateManager.container, templateContainer);
        unmatchedElementsInGoal = GetUnmatchedFromGoal(StateManager.container, templateContainer);
        unmatchedInGoalCopy = CopyElements(unmatchedElementsInGoal, transform);
        GetElement(unmatchedInGoalCopy, "congrats").GameObject.SetActive(false);
        StartCoroutine(MoveForm(clip));
        MoveVan();
        StartCoroutine(RemoveUnmatchedFromStart());
        
    }

    private IEnumerator MoveForm(IMorphState form)
    {
        form.MorphTo(formFirstCheckpoint.gameObject, hideElementsDuration, hideElementsEase);
        yield return new WaitForSeconds(formFirstCheckDuration);
        Destroy(form.GameObject);
    }

    private void MoveVan()
    {
        var van = GetElement(unmatchedInGoalCopy, "van");
        var furniture = GetElement(StateManager.container, "3d");
        var form = GetElement(StateManager.container, "form");
        if (van != null && furniture != null)
        {
            var moveIn = DOTween.Sequence()
                .Append(van.GameObject.transform
                    .DOMove(vanFirstCheckpoint.position, vanMoveInDuration)
                    .SetEase(vanMoveInEase))
                .Append(furniture.GameObject.transform
                    .DOMove(furnitureFirstCheckpoint.position, furnitureFirstMoveDuration)
                    .SetEase(furnitureFirstEase))
                .Append(furniture.GameObject.transform
                    .DOMove(furnitureSecondCheckPoint.position, furnitureSecondMoveDuration)
                    .SetEase(furnitureSecondEase))
                .Insert(1,
                    furniture.GameObject.transform
                        .DOScale(furnitureSecondCheckPoint.localScale, furnitureSecondMoveDuration)
                        .SetEase(furnitureSecondEase));
            moveIn.OnComplete(Rest);
        }
    }

    private void Rest()
    {
        var van = GetElement(unmatchedInGoalCopy, "van");
        var furniture = GetElement(StateManager.container, "3d");
        StartCoroutine(EnableCongrats());
        furniture.GameObject.transform.position = new Vector3(0,10,-100);
        furniture.GameObject.transform.localScale = new Vector3(0,0,0);
        furniture.GameObject.transform.eulerAngles = new Vector3(furniture.GameObject.transform.rotation.x,0, furniture.GameObject.transform.rotation.z);
        var moveOff = van.GameObject.transform.DOMove(vanSecondCheckpoint.position, vanSecondDuration).SetEase(vanSecondEase);
        moveOff.OnComplete(LoadHome);
    }

    private IEnumerator EnableCongrats()
    {
        var congrats = GetElement(unmatchedInGoalCopy, "congrats");
        yield return new WaitForSeconds(congratsDelay);
        congrats.GameObject.SetActive(true);
        
    }
    private void LoadHome()
    {
        StartCoroutine(LoadAfterDelay());
    }

    private IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(loadHomeAfter);
        EnableElements(unmatchedElementsInStart, true);
        StateManager.SetState(StateManager.homeState, StateManager.container);
    }
    private IEnumerator RemoveUnmatchedFromStart()
    {
        unmatchedElementsInStart = GetUnmatchedFromStart(StateManager.container, templateContainer);
        foreach (var element in unmatchedElementsInStart)
        {
            Vector3 upDistance = new Vector3(0,25,0);
            element.MoveTo(element.GameObject.transform.position + upDistance, hideElementsDuration, hideElementsEase);
        }
        yield return new WaitForSeconds(hideElementsDuration);
        EnableElements(unmatchedElementsInStart, false);
    }

    public override void Hide()
    {
        UnsetTransform(gameObject);
        EnableContainer(templateContainer, true);
    }
}
