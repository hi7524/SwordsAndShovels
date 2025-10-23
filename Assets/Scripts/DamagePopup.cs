using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float moveSpeed = 1.0f;
    public float fadeSpeed = 2.0f;
    public float lifeTime = 1.0f;

    private Color textColor;

    public void Setup(int damage)
    {
        textMesh.text = damage.ToString();
        textColor = textMesh.color;
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        textColor.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = textColor;

        if (textColor.a <= 0)
            Destroy(gameObject);
    }
}
