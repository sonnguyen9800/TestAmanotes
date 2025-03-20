using TestAmanotes;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NoteDestroyer : MonoBehaviour
{
    [SerializeField] private LayerMask _layerToBeDestroyed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _layerToBeDestroyed) != 0)
        {
            NoteSpawnerManager.Instance.DestroyNote(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // You can add additional logic here if needed, for now, it does nothing.
    }
}