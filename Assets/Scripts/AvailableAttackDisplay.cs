using UnityEngine;
using UnityEngine.UI;

public class AvailableAttackDisplay : MonoBehaviour
{
    [SerializeField]
    private Image[] images;
    [SerializeField]
    private TMPro.TMP_Text damageLabel;

    public void Clear()
    {
        foreach (var image in images)
            image.enabled = false;
        damageLabel.enabled = false;
    }

    public void SetSigil(Sigil sigil)
    {
        foreach (var image in images)
            image.enabled = false;

        for (int i = 0; i < sigil.Shape.Count; i++)
        {
            var image = images[i];
            image.enabled = true;
            image.sprite = sigil.Shape[i].Sprite;
            image.color = sigil.Shape[i].Color;
        }

        damageLabel.enabled = true;
        damageLabel.text = "Damage " + sigil.Damage;
    }
}
