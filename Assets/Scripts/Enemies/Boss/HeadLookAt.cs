using System;
using Loopie;

class HeadLookAt : Component
{
    public Entity head;
    public bool active;

    void OnCreate()
    {
    }

    void OnUpdate()
    {
        if (active)
        {
            head.transform.LookAt(Player.Instance.entity.transform.position, Vector3.Up);
        }else
            head.transform.local_rotation = Vector3.Zero;
    }
};