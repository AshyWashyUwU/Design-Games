using UnityEngine;

public class TransformationDeviceScanningRangeHandler : MonoBehaviour
{
    [SerializeField] private CreatureData _storedCreatureData;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        ICreature _creatureComponent = _collision.GetComponent<ICreature>();

        if (_creatureComponent != null)
        {
            _storedCreatureData = _creatureComponent.GetCreatureData();
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        ICreature _creatureComponent = _collision.GetComponent<ICreature>();

        if (_creatureComponent.GetCreatureData() == _storedCreatureData)
        {
            _storedCreatureData = null;
        }
    }
}
