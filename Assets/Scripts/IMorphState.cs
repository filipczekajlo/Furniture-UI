using DG.Tweening;
using UnityEngine;

public interface IMorphState // I don't need an Interface for now.
{
    string Name { get; }
    GameObject GameObject { get;  }

    void MorphTo(GameObject goal, float moveDuration, Ease ease);
    void MoveTo(Vector3 position, float moveDuration, Ease ease);
    void RotateTo(Vector3 rotation, float moveDuration, Ease ease);
}
