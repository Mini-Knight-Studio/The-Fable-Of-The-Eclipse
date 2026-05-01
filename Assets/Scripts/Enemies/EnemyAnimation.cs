using System;
using System.Threading;
using Loopie;
public class EnemyAnimation : Component
{

    public Entity model;
    private Animator animator;
    private bool clip_ended;
    private bool stay_until_finish;
    private float animation_timer;

    void OnCreate()
    {
        animator = model.GetComponent<Animator>();
        clip_ended = false;
        stay_until_finish = false;
        animation_timer = 0.0f;
    }
    void OnUpdate()
    {
        if(animation_timer < animator.GetCurrentClipDuration())
            animation_timer += Time.deltaTime;
        clip_ended = (animation_timer >= animator.GetCurrentClipDuration());
        if (animation_timer >= animator.GetCurrentClipDuration() && animator.Looping)
            animation_timer -= animator.GetCurrentClipDuration();
    }

    public void PlayClip(string clip_name, bool loop, float transition, bool reset = false, bool stay_until_end = false)
    {
        if (stay_until_finish && !clip_ended) return;
        animator.Looping = loop;
        if (animator.GetCurrentClipName() == clip_name && !animator.InTransition && !reset) return;
        if (animator.GetNextClipName() == clip_name && animator.InTransition && !reset) return;
        animation_timer = 0;
        stay_until_finish = stay_until_end;
        animator.Play(clip_name, transition);
    }

    public string CurrentClip()
    {
        return animator.GetCurrentClipName();
    }

    public float ClipDuration()
    {
        return animator.GetCurrentClipDuration();
    }

    public bool AnimationEnded()
    {
        return clip_ended;
    }
};