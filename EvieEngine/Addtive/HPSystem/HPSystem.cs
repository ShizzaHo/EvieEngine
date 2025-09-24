using UnityEngine;
using System.Collections;

public abstract class HPSystem : MonoBehaviour
{
    [Header("HP Settings")]
    public float HP = 100f;
    public float MaxHP = 100f;

    [Header("Regeneration")]
    public bool allowRegenerate = false;
    public float regenerationRate = 1f;     
    public float regenerationDelay = 1f; 

    private Coroutine regenRoutine;

    public virtual void OnDamage(float amount)
    {
        HP -= amount;
        if (HP < 0) HP = 0;

        // Перезапускаем регенерацию (если включена)
        if (allowRegenerate)
        {
            if (regenRoutine != null) StopCoroutine(regenRoutine);
            regenRoutine = StartCoroutine(RegenerateHealth());
        }

        if (HP <= 0)
        {
            OnDeath();
        }
    }

    protected abstract void OnDeath();

    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(regenerationDelay);

        while (HP < MaxHP)
        {
            HP += regenerationRate * Time.deltaTime;
            if (HP > MaxHP) HP = MaxHP;
            yield return null;
        }

        regenRoutine = null;
    }

    public virtual void RestoreFull()
    {
        HP = MaxHP;
    }
}