using System;
using Loopie;

public class WobblyEffect : Component
{
    public float wobbleSpeed = 15.0f;
    public float wobbleAmount = 0.2f;

    private Vector3 lastPosition;
    private float wobbleTimer = 0.0f;
    private Vector3 baseScale;

    //public WobblyEffect() { }

    public void OnCreate()
    {

        lastPosition = entity.transform.position;
        baseScale = entity.transform.scale;


    }

    public void OnUpdate()
    {
        Vector3 currentPosition = entity.transform.position;


        float deltaX = currentPosition.x - lastPosition.x;
        float deltaY = currentPosition.y - lastPosition.y;
        float deltaZ = currentPosition.z - lastPosition.z;


        float moveDistanceSq = (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ);


        if (moveDistanceSq > 0.0001f)
        {

            wobbleTimer += Time.deltaTime * wobbleSpeed;


            float yWobble = (float)Mathf.Sin(wobbleTimer) * wobbleAmount;


            float xzWobble = -yWobble * 0.5f;


            entity.transform.scale = new Vector3(
                baseScale.x + xzWobble,
                baseScale.y + yWobble,
                baseScale.z + xzWobble
            );
        }
        else
        {

            Vector3 currentScale = entity.transform.scale;
            float lerpSpeed = 10.0f * Time.deltaTime;

            currentScale.x += (baseScale.x - currentScale.x) * lerpSpeed;
            currentScale.y += (baseScale.y - currentScale.y) * lerpSpeed;
            currentScale.z += (baseScale.z - currentScale.z) * lerpSpeed;

            entity.transform.scale = currentScale;


            wobbleTimer = 0.0f;
        }


        lastPosition = currentPosition;
    }
}


