using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystem : MonoBehaviour
{
    public AnimationPlayer Show;
    public AnimationPlayer Hide;

    public float ShowDuration;

    public void PlayShow()
    {
        StartCoroutine(CoDelay());
    }

    IEnumerator CoDelay()
    {
        Show.Play();
        yield return new WaitForSeconds(ShowDuration);
        Show.Stop();
    }
}
