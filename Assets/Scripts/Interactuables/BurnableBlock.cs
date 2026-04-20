using System;
using System.Collections;
using Loopie;

public class BurnableBlock : Component
{
    private BoxCollider myCollider;
    private bool isBurning = false;

    void OnCreate()
    {
        myCollider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (myCollider == null || isBurning) return;

        if (myCollider.IsColliding)
        {
            StartCoroutine(BurnSequence());
        }
    }

    private IEnumerator BurnSequence()
    {
        isBurning = true;

        float timer = 0.0f;
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        entity.SetActive(false);
    }
}