using System;
using Loopie;

class HealthSlot : Component
{
    public Entity fullSlotEntity;
    public Entity halfSlotEntity;

    public Image fullSlotImage;
    public Image halfSlotImage;

    void OnCreate()
    {
        fullSlotImage = fullSlotEntity.GetComponent<Image>();
        halfSlotImage = halfSlotEntity.GetComponent<Image>();
    }

    public void UpdateVisuals(int value)
    {
        fullSlotEntity.SetActive(value==2);
        halfSlotEntity.SetActive(value==1);
    }

    public void Lock()
    {
        halfSlotEntity.SetActive(false);
        fullSlotEntity.SetActive(true);
    }
}