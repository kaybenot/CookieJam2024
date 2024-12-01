using UnityEngine;
using UnityEngine.UI;

public class SmoothShow : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private float speed = 1;

    private void OnEnable()
    {
        var c = image.color;
        c.a = 0;
        image.color = c;
    }

    private void Update()
    {
        var c = image.color;
        c.a += speed * Time.deltaTime;
        image.color = c;
    }
}
