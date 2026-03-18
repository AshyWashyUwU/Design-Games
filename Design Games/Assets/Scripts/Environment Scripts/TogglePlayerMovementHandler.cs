using UnityEngine;

public class TogglePlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private Transform _playerTeleportPoint;

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.GetComponent<PlayerMovementHandler>())
        {
            _other.transform.position = _playerTeleportPoint.transform.position;

            PlayerMovementHandler _player = _other.GetComponent<PlayerMovementHandler>();

            _player.TogglePlayerMovementType();
        }
    }
}