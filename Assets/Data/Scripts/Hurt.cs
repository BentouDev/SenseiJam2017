using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hurt : MonoBehaviour
{
    public Damageable Owner;
    public int Damage;
    public bool Once;
    public bool OnEnter;
    public bool OnStay;
    public bool OnLeave;
    public float Delay;

    private float LastHurt;
    private bool DidHurt;

    public delegate void HurtEvent(Damageable dmg, Collider other);

    public event HurtEvent OnHurt;
    public UnityEvent OnDidHurt;

    public GameObject Effect;
    public float Lifetime = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (!OnEnter)
            return;

        var dmg = other.GetComponentInChildren<Damageable>() ?? other.GetComponentInParent<Damageable>();
        if (dmg)
            DoHurt(dmg, other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!OnLeave)
            return;

        var dmg = other.GetComponentInChildren<Damageable>() ?? other.GetComponentInParent<Damageable>();
        if (dmg)
            DoHurt(dmg, other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!OnStay)
            return;

        var dmg = other.GetComponentInChildren<Damageable>() ?? other.GetComponentInParent<Damageable>();
        if (dmg)
            DoHurt(dmg, other);
    }
    
    public void DoHurt(Damageable damageable, Collider other)
    {
        if (damageable == Owner)
            return;

        if (Time.time - LastHurt < Delay)
            return;

        if (Once && DidHurt)
            return;

        LastHurt = Time.time;
        DidHurt = true;
        
        OnDidHurt.Invoke();
        if (Effect)
            SpawnEffect(other);

        damageable.Hurt(Damage);
        OnHurt?.Invoke(damageable, other);
    }

    public void SpawnEffect(Collider other)
    {
        var go = Instantiate(Effect, transform.position, Quaternion.identity);
        StartCoroutine(CoLife(go));
    }

    IEnumerator CoLife(GameObject go)
    {
        yield return new WaitForSeconds(Delay);
        Destroy(go);
    }
}
