using System;
using System.Threading;
using Loopie;

public class WobblyEffect : Component
{
    public float wobbleAmount = 0.2f;
    public float wobbleTime = 1.0f;

    private float timer = 0.0f;
    private Vector3 baseScale;

    void OnCreate()
    {
        Random rnd = new Random();
        timer = (float)(rnd.Next(0,100)/100.0f);
    }

    void OnUpdate()
    {
        timer += Time.deltaTime;

        float t = timer / wobbleTime;

        if (t > 1.0f)
        {
            t -= 1.0f;
            timer -= wobbleTime;
        }

        float yWobble = (float)Math.Sin(t * Math.PI * 2.0f) * wobbleAmount;
        float xzWobble = -yWobble * 0.5f;

        Vector3 targetScale = new Vector3(
            baseScale.x + xzWobble,
            baseScale.y + yWobble,
            baseScale.z + xzWobble
        );

        entity.transform.scale = Vector3.Lerp(
            entity.transform.scale,
            targetScale,
            t
        );
    }

    public void SetBaseScale(float newScale)
    {
        baseScale = Vector3.One * newScale;
    }
}
