using UnityEngine;

public class RangeBonus : MonoBehaviour
{
    [SerializeField] private int damageMultiplier;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            SfxManager.Instance.PlaySfx(Constants.BONUS_SFX);
            Bomb bombSeed = other.GetComponent<Player>().bombPrefab
                .GetComponent<Bomb>();

            bombSeed.BaseTileExplosionRange *= damageMultiplier;
            bombSeed.BaseWallExplosionRange *= damageMultiplier;
            
            Destroy(gameObject);
        }
    }
}