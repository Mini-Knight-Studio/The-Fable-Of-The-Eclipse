using System;
using System.Collections;
using Loopie;

class InteractHover : Component
{
    [Header("Settings")]
    public float amplitude = 0.5f;
    public float speed = 2.0f;
    public bool started = false;

    private Vector3 startLocalPos;
    private float time;

    void OnCreate()
    {
        StartMoving();
    }

    void OnUpdate()
    {
        if (!started) return;

        time += Time.deltaTime * speed;

        float offsetY = (float)Math.Sin(time) * amplitude;

        transform.local_position = new Vector3(startLocalPos.x, startLocalPos.y + offsetY, startLocalPos.z);
    }

    public void StartMoving()
    {
        startLocalPos = transform.local_position;
        time = 0f;

        started = true;
    }
};