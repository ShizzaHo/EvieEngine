using EvieEngine;
using TMPro;
using UnityEngine;

public class EvieDemo_michenHP : HPSystem
{
    public TMP_Text text;

    public override void OnDamage(float amount)
    {
        base.OnDamage(amount);
        text.text = HP+"";
    }

    protected override void OnDeath()
    {
        Debug.Log("Объект EvieDemo_michenHP уничтожен");
        Destroy(gameObject);
    }
}
