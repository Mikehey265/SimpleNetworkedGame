using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoCountText;
    [SerializeField] private TextMeshProUGUI healthPointsText;

    private void OnEnable()
    {
        PlayerAmmo.ChangedAmmoCountEvent += ChangeAmmoCountText;
        PlayerHealth.ChangedHealthPointsEvent += ChangeHealthPointsText;
    }

    private void OnDisable()
    {
        PlayerAmmo.ChangedAmmoCountEvent -= ChangeAmmoCountText;
        PlayerHealth.ChangedHealthPointsEvent -= ChangeHealthPointsText;
    }

    private void ChangeAmmoCountText(int count)
    {
        ammoCountText.text = "Ammo: " + count;
    }

    private void ChangeHealthPointsText(int count)
    {
        healthPointsText.text = "Health: " + count;
    }
}
