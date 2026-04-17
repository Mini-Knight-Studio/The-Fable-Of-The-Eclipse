using System;
using System.Collections;
using Loopie;

public class Movement : Component
{
    public float Speed;
    public bool CanMove;

    public void Move(Vector3 direction, float multiplier = 1)
    {
        if(CanMove)
            transform.position += direction * (Time.deltaTime * Speed * multiplier);
    }

    public void Move(float multiplier, Vector3 direction)
    {
        if(CanMove)
            transform.position += direction * Time.deltaTime * Speed * multiplier;
    }

    public IEnumerator Push(float force, float duration, Vector3 direction)
    {
        float timer = 0;
        while (timer < duration)
        {
            float factor = duration - timer;
            factor = Mathf.Clamp01(factor / duration);
            transform.position += direction * force * Time.deltaTime * factor;
            timer += Time.deltaTime;
            yield return null;
        }
    }
};