using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour, IMorphState
{
    [Header("String Reference! Watch out for casing!")] [SerializeField]
    private string name;
    public string Name => name;
    public Element elementScript => this;
    public GameObject GameObject => gameObject;
    
    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public RectTransform rect;
    [HideInInspector] public Image img; // Image is wrong reference !?!?! Or not?
    [HideInInspector] public CanvasGroup canvas;
    
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        canvas = GetComponent<CanvasGroup>();
    }
    
    public void MorphTo(GameObject goal, float moveDuration, Ease ease)
    {
        if(goal.transform != null)
        {
            transform.DOMove(goal.transform.position, moveDuration).SetEase(ease);
            transform.DORotateQuaternion(goal.transform.rotation, moveDuration).SetEase(ease);
            transform.DOScale(goal.transform.localScale, moveDuration).SetEase(ease);
        }

        UIMove(goal, moveDuration, ease);
    }

    public void MoveTo(Vector3 position, float moveDuration, Ease ease)
    {
        transform.DOMove(position, moveDuration).SetEase(ease);
    }

    public void ScaleTo(Vector3 scale, float moveDuration, Ease ease)
    {
        transform.DOScale(scale, moveDuration).SetEase(ease);
    }

    public void RotateTo(Vector3 rotation, float moveDuration, Ease ease)
    {
        transform.DORotate(rotation, moveDuration).SetEase(ease);
    }

    private void UIMove(GameObject goal, float moveDuration, Ease ease)
    {
        var goalGroup = goal.GetComponentInChildren<CanvasGroup>();
        if (canvas && goalGroup != null)
        {
            canvas.DOFade(goalGroup.alpha, moveDuration).SetEase(ease);
        }
        var goalText = goal.GetComponentInChildren<TextMeshProUGUI>();
        if (text && goalText != null)
        {
            text.DOFontSize(goalText.fontSize, moveDuration).SetEase(ease);
            text.DOFade(goalText.color.a, moveDuration).SetEase(ease);
        }
    
        var goalRect = goal.GetComponentInChildren<RectTransform>();
        if (rect && goalRect != null)
        {
            rect.DOSizeDelta(goalRect.sizeDelta, moveDuration).SetEase(ease);
        }
        
        var goalImg = goal.GetComponentInChildren<Image>();
        if (img && goalImg != null)
        {
            img.DOFade(goalImg.color.a, moveDuration).SetEase(ease);
            img.DOColor(goalImg.color, moveDuration).SetEase(ease);
        }
    }
}
