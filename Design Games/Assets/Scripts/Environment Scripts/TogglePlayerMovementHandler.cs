using UnityEngine;

public class TogglePlayerMovementHandler : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _playerTeleportPoint;

    [SerializeField] private string _customText;
    [SerializeField] private Vector3 _customOffset;

    public void Interact(GameObject _interactor)
    {
        PlayerMovementHandler._instance.GetPlayerTransform().position = _playerTeleportPoint.transform.position;

        PlayerMovementHandler._instance.TogglePlayerMovementType();
    }

    public string GetInteractText()
    {
        return _customText;
    }

    public Vector3 GetInteractOffset()
    {
        return _customOffset;
    }
}