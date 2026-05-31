using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerUpgrade
{
    public float _upgradeFloatAmount;
    public float _upgradeResearchCost;
}

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public List<PlayerUpgrade> _upgradeTier = new List<PlayerUpgrade>();

    public string _upgradeKey;
}
