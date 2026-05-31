using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _upgradeTier;
    [SerializeField] private TextMeshProUGUI _upgradeCost;

    [SerializeField] private Image _buttonImage;

    [SerializeField] private Color _cannotUpgradeColor;
    [SerializeField] private Color _canUpgradeColor;
    [SerializeField] private Color _maxedColor;

    [SerializeField] private UpgradeData _upgrade;

    public void UpdateButton(int _newUpgradeTier, float _newData)
    {
        _upgradeTier.text = "Tier " + _newUpgradeTier.ToString();

        if (_newData != 1)
        {
            _upgradeCost.text = _newData.ToString() + " GB";

            if (_newData > ComputerDataHolder._instance._totalUploadedData)
            {
                _buttonImage.color = _cannotUpgradeColor;
            }
            else
            {
                _buttonImage.color = _canUpgradeColor;
            }
        }
        else
        {
            _upgradeCost.text = "MAXED";

            _buttonImage.color = _maxedColor;
        }
    }

    public void PurchaseUpgrade()
    {
        UpgradeDataHolder._instance.Upgrade(_upgrade, false);
    }
}
