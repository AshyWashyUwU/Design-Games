using UnityEngine;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdatePlayerSwimSpeed(_playerSwimSpeed);
        UpdatePlayerDataMaximum(_playerDataMaximum);
        UpdatePlayerDataScanRange(_playerScanRange);
        UpdatePlayerCollectionAmount(_playerDataCollectionAmount);
    }

    public void UpdatePlayerSwimSpeed(float _newSwimSpeed)
    {
        _playerSwimSpeed = _newSwimSpeed;

        PlayerMovementHandler._instance.UpdateSwimSpeed(_newSwimSpeed);
    }

    public void UpdatePlayerDataMaximum(float _newDataMaximum)
    {
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
