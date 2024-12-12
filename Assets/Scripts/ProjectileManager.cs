using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public Projectile projectilePrefab;
    public Deactivable deativablePrefab;
    private ObjectPool<Projectile> projectilePool;
    private ObjectPool<Deactivable> deactivablePool;

    private void Start()
    {
        projectilePool = new ObjectPool<Projectile>(projectilePrefab, gameObject);
        deactivablePool = new ObjectPool<Deactivable>(deativablePrefab, gameObject);

        GameEvents.OnProjectileHit.AddListener(OnProjectileHit);
    }

    public Projectile GetProjectile()
    {
        return projectilePool.GetPooledObject();
    }

    public Deactivable GetDeactivable()
    {
        return deactivablePool.GetPooledObject();
    }

    private void OnProjectileHit(Vector3 position)
    {
        Deactivable destroyable = GetDeactivable();
        destroyable.transform.position = position;
        destroyable.gameObject.SetActive(true);
    }
}