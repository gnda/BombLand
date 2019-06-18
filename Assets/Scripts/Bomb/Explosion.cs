using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Destroyable>())
        {
            Destroy(other.gameObject);
        }
    }
}