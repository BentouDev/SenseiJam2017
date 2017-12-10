using Framework;
using UnityEngine;

[RequireComponent(typeof(Lifetime))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private void OnDisable()
    {
        if (Body)
            Body.velocity = Vector3.zero;
    }

    public StatePawn Owner;
    public Lifetime Life;
    public Rigidbody Body;

    public float SpeedupTime = 0.5f;

    public float Speed = 15;
    
    public void Begin()
    {
        this.TryInit(ref Life);
        this.TryInit(ref Body);
        
        Life.Begin();
    }

    void Update()
    {
        var coeff = Mathf.Clamp01(Life.Elapsed / SpeedupTime);
        Body.velocity = transform.forward * Speed * Mathf.Sin(coeff) + Vector3.Lerp(Owner.Velocity, Vector3.zero, coeff);
    }
    
    
}