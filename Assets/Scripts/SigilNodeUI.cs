using UnityEngine;

public class SigilNodeUI : MonoBehaviour
{
    public SigilDescriptionUI DescriptionUI { get; set; }
    public SigilTreeNode Node { get; set; }

    public void OnClick()
    {
        DescriptionUI.Show(Node);
    }
}
