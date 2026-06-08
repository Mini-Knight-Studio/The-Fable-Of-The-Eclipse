using System;
using Loopie;

class HealthSlot : Component
{
    public Entity fullSlotEntity;
    public Entity halfSlotEntity;

    public Image fullSlotImage;

    void OnCreate()
    {
        fullSlotImage = fullSlotEntity.GetComponent<Image>();
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
        fullSlotImage.SetTint(new Vector4(20f, 20f, 20f, 1f));
    }

    public void Unlock()
    {
        fullSlotImage.SetTint(new Vector4(1,1,1,1));
    }
}