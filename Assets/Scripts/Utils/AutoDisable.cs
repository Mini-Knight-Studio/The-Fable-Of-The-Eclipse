using System;
using System.Collections;
using Loopie;

public class AutoDisable : Component
{
    public float lifetime = 1.5f;
    private float timer = 0f;

    void OnCreate()
    {
    }

    void OnUpdate()
    {
        
        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            timer = 0f; 
            entity.SetActive(false); 
        }
    }
}; 