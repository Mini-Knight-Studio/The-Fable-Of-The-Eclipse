using System;
using Loopie;

public class Enemy : Component
{
    protected Entity target;
    protected void SetTarget(string name)
    {
        target = Entity.FindEntityByName(name);
        if (target == null)
        {
            Debug.LogWarning("Cannot find target entity");
        }
        else
        {
            Debug.LogWarning("Entity found");
        }
    }
}


