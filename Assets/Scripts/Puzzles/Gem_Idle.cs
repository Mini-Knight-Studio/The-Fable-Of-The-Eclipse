using System;
using Loopie;

class Gem_Idle : Component
{
    private Vector3 startLocalPos;
    private float time;

    public float amplitude = 0.5f;
    public float speed = 2.0f;

    public Entity interactionPrompt;

    void OnCreate()
    {
        startLocalPos = transform.local_position;
        time = 0f;

        interactionPrompt.SetActive(false);
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

        time += Time.deltaTime * speed;

        float offsetY = (float)Math.Sin(time) * amplitude;

        transform.local_position = new Vector3(startLocalPos.x, startLocalPos.y + offsetY, startLocalPos.z);
    }
};