using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FormState : State
{

    [SerializeField] private Container templateContainer;

    [SerializeField] private float moveUpDuration = 0.4f;
    [SerializeField] private float formMoveUpOffset = 0;


    [SerializeField] private Ease moveUpEase;
    
    public override void Show()
    {
        EnableContainer(templateContainer, false);
        SetTransform(gameObject, distanceFromViewCanvas);
        ChangeParent(StateManager.container, transform);
        foundElements = GetMatchedElements(StateManager.container, templateContainer);
        unmatchedElementsInStart = GetUnmatchedFromStart(StateManager.container, templateContainer);
        unmatchedElementsInGoal = GetUnmatchedFromGoal(StateManager.container, templateContainer);
        unmatchedInGoalCopy = CopyElements(unmatchedElementsInGoal);
        ChangeParent(unmatchedInGoalCopy, transform);
        MoveElements(foundElements, moveUpDuration, moveUpEase);
        EnableElements(unmatchedElementsInStart, false);
        UnsetPreviousScreen();
        MoveInClipBoard();
    }

    private void UnsetPreviousScreen()
    {
        print(StateManager.previousState);
        UnsetTransform(StateManager.previousState.gameObject);
    }
    
    private void MoveInClipBoard()
    {
        var form = GetElement(unmatchedInGoalCopy, "form");
        var obj = GetElement(StateManager.container, "3d");
        var position = transform.position;
        form.MoveTo(new Vector3(position.x, position.y + formMoveUpOffset, form.GameObject.transform.position.z), moveUpDuration, moveUpEase);
        form.RotateTo(new Vector3(90, -180, form.GameObject.transform.rotation.z), moveUpDuration, moveUpEase);
    }

    public void ReturnToProductView()
    {
        StateManager.SetState(StateManager.productState);
    }
    public void ShipButton()
    {
        StateManager.SetState(StateManager.completionState);
    }
    
    public override void Hide()
    {
        if (StateManager.currentState != StateManager.completionState)
        {
            var form = GetElement(unmatchedElementsInGoal, "form");
            var formCopy = GetElement(unmatchedInGoalCopy, "form"); 
            formCopy.MoveTo(new Vector3(form.GameObject.transform.position.x, form.GameObject.transform.position.y, form.GameObject.transform.position.z), moveUpDuration, moveUpEase);
            formCopy.RotateTo(new Vector3(form.GameObject.transform.rotation.x, -180, form.GameObject.transform.rotation.z), moveUpDuration, moveUpEase);
            StartCoroutine(HideAfterTransition());
            return;
        }
        UnsetTransform(gameObject);
        EnableContainer(templateContainer, true);
    }
    
    private IEnumerator HideAfterTransition()
    {
        yield return new WaitForSeconds(moveUpDuration);
        foreach (var element in unmatchedInGoalCopy)
        {
            //Destroy(element.GameObject);
        }
        UnsetTransform(gameObject);
        EnableContainer(templateContainer, true);
    }
    
}
