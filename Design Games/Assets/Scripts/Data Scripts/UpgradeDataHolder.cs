using UnityEngine;
using System.Collections.Generic;

public class UpgradeDataHolder : MonoBehaviour
{
    private static UpgradeDataHolder Instance;
    public static UpgradeDataHolder _instance { get => Instance; }

    public float _playerSwimSpeed { get; private set; } = 3f;

    public float _playerDataMaximum { get; private set; } = 100f;
    public float _playerDataCollectionAmount { get; private set; } = 0.01f;
    public float _playerScanRange { get; private set; } = 7f;

    public float _playerTransformationCooldown { get; private set; } = 30f;
    public float _playerTransformationLimit { get; private set; } = 45f;

    public float _computerUploadTimeMultiplier { get; private set; } = 1f;

    [SerializeField] private List<UpgradeData> _upgradeData = new List<UpgradeData>();
    [SerializeField] private List<int> _upgradeDataTiers = new List<int>();
    [SerializeField] private List<UpgradeButton> _upgradeButtons = new List<UpgradeButton>();

    [SerializeField] private UpgradeData _test;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void ActuallyUpgrade(string _upgradeKey, int _upgradeTier, float newData, bool _refreshing)
    {
        if (_upgradeKey == "PlayerSwimSpeed")
        {
            UpdatePlayerSwimSpeed(newData);
        }
        else if (_upgradeKey == "PlayerDataStorage")
        {
            UpdatePlayerDataMaximum(newData, _refreshing);
        }
        else if (_upgradeKey == "PlayerCollectionSpeed")
        {
            UpdatePlayerCollectionAmount(newData);
        }
        else if (_upgradeKey == "PlayerScanRange")
        {
            UpdatePlayerDataScanRange(newData);
        }
        else if (_upgradeKey == "TransformationCooldown")
        {
            _playerTransformationLimit = newData;
        }
        else if (_upgradeKey == "DetransformTimer")
        {
            _playerTransformationCooldown = newData;
        }
        else if (_upgradeKey == "ComputerDataUploadTime")
        {
            _computerUploadTimeMultiplier = newData;
        }
    }

    private void Start()
    {
        for (int i = 0; i < _upgradeData.Count; i++)
        {
            _upgradeDataTiers.Add(0);
        }

        for (int i = 0; i < _upgradeData.Count; i++)
        {
            Upgrade(_upgradeData[i], false);
        }
    }

    public void RefreshUpgrades()
    {
        for (int i = 0; i < _upgradeData.Count; i++)
        {
            Upgrade(_upgradeData[i], true);
        }
    }

    public void Upgrade(UpgradeData _chosenUpgrade, bool _refreshing)
    {
        int _upgradeIndex = 0;

        for (int i = 0; i < _upgradeData.Count; i++)
        {
            if (_upgradeData[i] == _chosenUpgrade)
            {
                _upgradeIndex = i;
            }
        }

        int _upgradeTier = _upgradeDataTiers[_upgradeIndex];

        if (_upgradeData[_upgradeIndex]._upgradeTier.Count + 1 < _upgradeTier) return;

        float _upgradeTierCost = _upgradeData[_upgradeIndex]._upgradeTier[_upgradeTier]._upgradeResearchCost;

        if ((_upgradeTierCost > ComputerDataHolder._instance._totalUploadedData && !_refreshing) || _upgradeTierCost == 1)
        {
            return;
        }
        else
        {
            string _upgradeKey = _upgradeData[_upgradeIndex]._upgradeKey;

            if (_upgradeTierCost != 0 && !_refreshing) _upgradeDataTiers[_upgradeIndex]++;

            int _newTier = _upgradeDataTiers[_upgradeIndex];

            float newData = _upgradeData[_upgradeIndex]._upgradeTier[_newTier]._upgradeFloatAmount;

            ActuallyUpgrade(_upgradeKey, _newTier, newData, _refreshing);

            UpgradeButton _upgradeButton = _upgradeButtons[_upgradeIndex];

            if (_upgradeTierCost == 0 && !_refreshing) _upgradeDataTiers[_upgradeIndex]++;

            _newTier = _upgradeDataTiers[_upgradeIndex];

            float _newUpgradeTierCost = 0;

            if (_upgradeData[_upgradeIndex]._upgradeTier.Count >= _newTier)
            {
                _newUpgradeTierCost = _upgradeData[_upgradeIndex]._upgradeTier[_newTier]._upgradeResearchCost;
            }

            _upgradeButton.UpdateButton(_newTier, _newUpgradeTierCost);
        }

        if (!_refreshing) RefreshUpgrades();
    }

    public void UpdatePlayerSwimSpeed(float _newSwimSpeed)
    {
        _playerSwimSpeed = _newSwimSpeed;

        PlayerMovementHandler._instance.UpdateSwimSpeed(_newSwimSpeed);
    }

    public void UpdatePlayerDataMaximum(float _newDataMaximum, bool _refreshing)
    {
        if (_refreshing) return;

        _playerDataMaximum = _newDataMaximum;

        TransformationDeviceScanningHandler._instance.UpdateDataMaximum(_newDataMaximum);
    }

    public void UpdatePlayerDataScanRange(float _newScanRange)
    {
        _playerScanRange = _newScanRange;

        TransformationDeviceScanningHandler._instance.UpdateDataScanRange(_newScanRange);
    }

    public void UpdatePlayerCollectionAmount(float _newCollectionAmount)
    {
        _playerDataCollectionAmount = _newCollectionAmount;

        TransformationDeviceScanningHandler._instance.UpdateDataCollectionAmount(_newCollectionAmount);
    }
}
