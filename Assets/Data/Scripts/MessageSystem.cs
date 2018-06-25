using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MessageSystem : MonoBehaviour
{
    public AnimationPlayer Show;
    public AnimationPlayer Hide;

    public PlayableDirector ShowPlayable;

    public float ShowDuration;

    public void PlayShow()
    {
        StartCoroutine(CoDelay());
    }

    IEnumerator CoDelay()
    {
        yield return new WaitForEndOfFrame();
        Show.Play();
        ShowPlayable?.Play();
        yield return new WaitForSeconds(ShowDuration);
        Show.Stop();
        ShowPlayable?.Stop();
    }
}
