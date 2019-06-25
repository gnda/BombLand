using UnityEngine;

public class SwitchBombBonus : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            SfxManager.Instance.PlaySfx(Constants.BONUS_2_SFX);
            bombPrefab.GetComponent<Bomb>().Player = other.GetComponent<Player>();
            other.GetComponent<Player>().bombPrefab = bombPrefab;
            
            Destroy(gameObject);
        }
    }
}