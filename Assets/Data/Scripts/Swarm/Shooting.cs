using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public bool InitOnStart;
    public AudioRandomizer Audio;
    
    public StatePawn Pawn;
    public GameObject MuzzlePrefab;
    private Effect Muzzle;
    
    private float StandardCooldown;
    public float MinRandomCooldown;
    public float MaxRandomCooldown;
    public List<Rifle> Rifles = new List<Rifle>();

    private float LastShootTime;

    private int RifleIndex;

    private void Start()
    {
        if (InitOnStart)
            Init();
    }

    public void Init()
    {
        if (!Pawn)
            Pawn = GetComponentInChildren<StatePawn>() ?? GetComponentInParent<StatePawn>();
        
        Rifles.AddRange(GetComponentsInChildren<Rifle>());
        
        LastShootTime = Time.time;
        StandardCooldown = Random.Range(MinRandomCooldown, MaxRandomCooldown);
    }

    public void Shoot(PrefabPool projectile)
    {
        if (!Rifles.Any())
            return;

        if ((Time.time - LastShootTime) / Rifles.Count < StandardCooldown)
            return;

        LastShootTime = Time.time;
        StandardCooldown = Random.Range(MinRandomCooldown, MaxRandomCooldown);

        if (!Muzzle)
        {
            Muzzle = Instantiate(MuzzlePrefab).GetComponent<Effect>();
            Muzzle.transform.position = Rifles[RifleIndex].transform.position;
            Muzzle.transform.SetParent(Rifles[RifleIndex].transform, true);
        }
        
        Muzzle.Begin();
        
        var obj = projectile.GetObject();
        if (!obj)
            return;

        obj.transform.position = Rifles[RifleIndex].transform.position;
        obj.transform.forward = transform.forward;
        obj.SetActive(true);
        obj.transform.position = Rifles[RifleIndex].transform.position;
        obj.transform.forward = transform.forward;
        
        var instance = obj.GetComponent<Projectile>();
        instance.Owner = Pawn;
        instance.Begin();
        
        Audio.Play();
        
        RifleIndex++;
        if (RifleIndex >= Rifles.Count)
            RifleIndex = 0;
    }
}
