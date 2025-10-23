using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
    public static DamagePopupSpawner Instance { get; private set; }

    
    public GameObject popupPrefab;

    public float yOffset = 2f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowPopup(Vector3 position, int damage)
    {
        if (popupPrefab == null)
        {
            Debug.LogWarning("[DamagePopupSpawner] Popup Prefab is not assigned!");
            return;
        }

        Vector3 spawnPos = position + Vector3.up * yOffset;
        var popup = Instantiate(popupPrefab, spawnPos, Quaternion.identity);
        popup.transform.LookAt(Camera.main.transform);
        popup.transform.Rotate(0, 180f, 0);
        popup.GetComponent<DamagePopup>().Setup(damage);
    }
}