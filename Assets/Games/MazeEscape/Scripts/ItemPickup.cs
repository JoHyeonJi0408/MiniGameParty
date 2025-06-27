using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        itemData.effect?.Apply(MazeEscape.GameManager.Instance);
        Destroy(gameObject);
    }
}
