using System;
using Loopie;

class HealthSlot : Component
{
    public Entity fullSlotEntity;
    public Entity halfSlotEntity;

    public Image fullSlotImage;
    public Image halfSlotImage;

    private Vector4 originalFullTint = Vector4.One;
    private Vector4 originalHalfTint = Vector4.One;

    void OnCreate()
    {
        fullSlotImage = fullSlotEntity.GetComponent<Image>();
        if (fullSlotImage != null)
        {
            originalFullTint = fullSlotImage.GetTint();
        }

        halfSlotImage = halfSlotEntity.GetComponent<Image>();
        if (halfSlotImage != null)
        {
            originalHalfTint = halfSlotImage.GetTint();
        }
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
        if (fullSlotImage != null)
        {
            fullSlotImage.SetTint(new Vector4(0.08f, 0.08f, 0.08f, 1f));
        }
    }

    public void Unlock()
    {
        if (fullSlotImage != null)
        {
            fullSlotImage.SetTint(originalFullTint);
        }
        if (halfSlotImage != null)
        {
            halfSlotImage.SetTint(originalHalfTint);
        }
    }
}