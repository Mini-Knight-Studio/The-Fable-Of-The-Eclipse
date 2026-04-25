using System;
using Loopie;

class HeadLookAt : Component
{
    public Entity head;
    private Entity inner_head1;
    private Entity inner_head2;
    public bool active;

    void OnCreate()
    {
        inner_head1 = head.GetChildByName("Head_Base");
        inner_head2 = head.GetChildByName("Head_Rotated");
    }

    void OnUpdate()
    {
        if (active)
        {
            head.transform.LookAt(Player.Instance.entity.transform.position, Vector3.Up);
            inner_head1.SetActive(false);
            inner_head2.SetActive(true);
        }
        else
        {
            inner_head2.SetActive(false);
            inner_head1.SetActive(true);
        }
    }
};