using UnityEngine;

public class NametagHandler : MonoBehaviour
{
    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;

        if (transform.parent != null)
        {
            Vector3 parentScale = transform.parent.lossyScale;

            transform.localScale = new Vector3( initialScale.x / parentScale.x, initialScale.y / parentScale.y, initialScale.z / parentScale.z);
        }
        else
        {
            transform.localScale = initialScale;
        }
    }
}