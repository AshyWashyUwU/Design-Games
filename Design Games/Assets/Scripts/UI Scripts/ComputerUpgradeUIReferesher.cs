using UnityEngine;

public class ComputerUpgradeUIReferesher : MonoBehaviour
{
    private void OnEnable()
    {
        UpgradeDataHolder._instance.RefreshUpgrades();
    }
}
