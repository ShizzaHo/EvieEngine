using UnityEngine;

public class SimpleBullet : Bullet
{
    public override void Shoot(Vector3 position, Vector3 direction)
    {
        base.Shoot(position, direction);

        GetComponentInChildren<TrailRenderer>().Clear();
    }

    protected override void OnHit(Collision collision)
    {
        HPSystem hp = collision.gameObject.GetComponentInParent<HPSystem>();
        if (hp != null)
        {
            hp.OnDamage(damage);
        }
    }
}