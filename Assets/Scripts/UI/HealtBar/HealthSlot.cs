using System;
using Loopie;

class HealthSlot : Component
{
    public Entity fullSlotEntity;
    public Entity halfSlotEntity;


    public void UpdateVisuals(int value)
    {
        fullSlotEntity.SetActive(value==2);
        halfSlotEntity.SetActive(value==1);
    }


}