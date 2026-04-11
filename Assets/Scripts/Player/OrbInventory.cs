using System;
using Loopie;

public class OrbInventory : Component
{
    public int orbsCollected = 0;
    public int maxOrbs = 3;

    void OnCreate()
    {
        orbsCollected = 0;
    }

    void OnUpdate()
    {
    }

   
    public void AddOrb()
    {
        if (orbsCollected < maxOrbs)
        {
            orbsCollected++;
            Debug.Log("¡Orbe recogido! Total: " + orbsCollected);
        }
    }
}; 