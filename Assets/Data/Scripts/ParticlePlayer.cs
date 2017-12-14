using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    public ParticleSystem Particle;

    public void Start()
    {
        Begin();
    }
    
    public void Begin()
    {
        this.TryInit(ref Particle);
        Particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Particle.Play();
    }
}
