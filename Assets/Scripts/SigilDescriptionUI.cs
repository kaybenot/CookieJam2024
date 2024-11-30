using TMPro;
using UnityEngine;

public class SigilDescriptionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text buyCost;
    [SerializeField] private TMP_Text castCost;
    [SerializeField] private TMP_Text unlocked;

    public void Show(SigilTreeNode node)
    {
        title.text = node.Sigil.Name;
        buyCost.text = $"Buy price (max HP): {node.BuyHpCost}";
        castCost.text = $"Cast HP cost: {node.Sigil.HealthCost}";
        unlocked.text = node.Unlocked ? "Unlocked" : "Not unlocked";
        
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
