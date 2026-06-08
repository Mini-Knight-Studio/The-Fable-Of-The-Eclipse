using System;
using System.Collections;
using Loopie;

class HealthSlot : Component
{
    public Entity fullSlotEntity;
    public Entity halfSlotEntity;

    public Image fullSlotImage;
    private Image halfSlotImage;

    private Image FullSlotImage
    {
        get
        {
            if (fullSlotImage == null || string.IsNullOrEmpty(fullSlotImage.ID))
            {
                if (fullSlotEntity != null)
                {
                    fullSlotImage = fullSlotEntity.GetComponent<Image>();
                }
            }
            return fullSlotImage;
        }
    }

    private Image HalfSlotImage
    {
        get
        {
            if (halfSlotImage == null || string.IsNullOrEmpty(halfSlotImage.ID))
            {
                if (halfSlotEntity != null)
                {
                    halfSlotImage = halfSlotEntity.GetComponent<Image>();
                }
            }
            return halfSlotImage;
        }
    }

    private void SetImageTint(Image img, Vector4 tint)
    {
        if (img != null && !string.IsNullOrEmpty(img.ID))
        {
            img.SetTint(tint);
        }
    }

    private Entity animEntity;
    private SpriteAnimator slotAnimator;
    private bool hasAnimator = false;

    private int currentValue = -1;
    private bool isLocked = false;
    private bool isAnimating = false;

    // Animation texture UUIDs
    private const string LIFE_LOSE1_UUID = "bdf83f12-7b77-4549-ba6a-4cbe4c688016";
    private const string LIFE_LOSE2_UUID = "1a0c2ded-ed5c-471b-adc5-b2c301a19156";
    private const string LIFE_GAIN_UUID  = "0ae7c098-f4af-44b6-8c4f-ecd89a801c2a";

    void OnCreate()
    {
        if (fullSlotEntity != null)
            fullSlotImage = fullSlotEntity.GetComponent<Image>();
        if (halfSlotEntity != null)
            halfSlotImage = halfSlotEntity.GetComponent<Image>();

        animEntity = entity.GetChildByName("Anim");
        if (animEntity != null && animEntity.HasComponent<SpriteAnimator>())
        {
            slotAnimator = animEntity.GetComponent<SpriteAnimator>();
            hasAnimator = true;
            animEntity.SetActive(false);
        }
    }

    public void UpdateVisuals(int value)
    {
        int oldValue = currentValue;
        currentValue = value;

        if (oldValue >= 0 && value < oldValue && !isAnimating)
        {
            if (hasAnimator)
            {
                StartCoroutine(PlayLoseAnimation(oldValue - value));
            }
            else
            {
                StartCoroutine(FlashLoseAnimation(oldValue, value));
            }
        }
        else if (oldValue >= 0 && value > oldValue && !isAnimating)
        {
            if (hasAnimator)
            {
                StartCoroutine(PlayGainAnimation(value));
            }
            else
            {
                StartCoroutine(FlashGainAnimation(value));
            }
        }
        else if (!isAnimating)
        {
            ApplyVisuals(value);
        }
    }

    private void ApplyVisuals(int value)
    {
        if (fullSlotEntity != null) fullSlotEntity.SetActive(value == 2);
        if (halfSlotEntity != null) halfSlotEntity.SetActive(value == 1);
    }

    public void Lock()
    {
        if (isLocked) return;
        isLocked = true;
        if (halfSlotEntity != null) halfSlotEntity.SetActive(false);
        if (fullSlotEntity != null) fullSlotEntity.SetActive(false);
        currentValue = 0;
    }

    public void Unlock()
    {
        if (isLocked)
        {
            SetImageTint(FullSlotImage, new Vector4(1, 1, 1, 1));
            SetImageTint(HalfSlotImage, new Vector4(1, 1, 1, 1));
            isLocked = false;
        }
    }

    public void FlashTint(bool bright)
    {
        if (fullSlotEntity != null)
        {
            fullSlotEntity.SetActive(true);
        }
        if (bright)
            SetImageTint(FullSlotImage, new Vector4(3, 3, 3, 1));
        else
            SetImageTint(FullSlotImage, new Vector4(1, 1, 1, 1));
    }

    public void ResetTint()
    {
        SetImageTint(FullSlotImage, new Vector4(1, 1, 1, 1));
    }

    private IEnumerator PlayLoseAnimation(int amountLost)
    {
        isAnimating = true;

        if (amountLost >= 2)
        {
            slotAnimator.TextureUUID = LIFE_LOSE2_UUID;
            slotAnimator.SetGrid(3, 1);
            slotAnimator.FrameCount = 2;
            slotAnimator.FPS = 8;
        }
        else
        {
            slotAnimator.TextureUUID = LIFE_LOSE1_UUID;
            slotAnimator.SetGrid(3, 1);
            slotAnimator.FrameCount = 3;
            slotAnimator.FPS = 8;
        }

        slotAnimator.StartFrame = 0;
        slotAnimator.Loop = false;

        if (fullSlotEntity != null) fullSlotEntity.SetActive(false);
        if (halfSlotEntity != null) halfSlotEntity.SetActive(false);
        animEntity.SetActive(true);
        slotAnimator.Play();

        float duration = (float)slotAnimator.FrameCount / slotAnimator.FPS;
        yield return new WaitForSeconds(duration + 0.05f);

        animEntity.SetActive(false);
        slotAnimator.Stop(true);

        isAnimating = false;
        ApplyVisuals(currentValue);
    }

    private IEnumerator FlashLoseAnimation(int oldVal, int newVal)
    {
        isAnimating = true;
        ApplyVisuals(oldVal);
        Image activeImage = (oldVal == 2) ? FullSlotImage : HalfSlotImage;

        SetImageTint(activeImage, new Vector4(1, 1, 1, 1));
        yield return new WaitForSeconds(0.1f);
        SetImageTint(activeImage, new Vector4(1.3f, 0.95f, 0.9f, 0.8f));
        yield return new WaitForSeconds(0.1f);
        SetImageTint(activeImage, new Vector4(1.5f, 1.1f, 1.05f, 0.5f));
        yield return new WaitForSeconds(0.1f);
        SetImageTint(activeImage, new Vector4(1.5f, 1.1f, 1.05f, 0.2f));
        yield return new WaitForSeconds(0.08f);

        SetImageTint(activeImage, new Vector4(1, 1, 1, 1));
        isAnimating = false;
        ApplyVisuals(currentValue);
    }

    private IEnumerator PlayGainAnimation(int newVal)
    {
        isAnimating = true;

        slotAnimator.TextureUUID = LIFE_GAIN_UUID;
        slotAnimator.SetGrid(13, 1);
        slotAnimator.FrameCount = 13;
        slotAnimator.FPS = 18;
        slotAnimator.StartFrame = 0;
        slotAnimator.Loop = false;

        ApplyVisuals(newVal);
        animEntity.SetActive(true);
        slotAnimator.Play();

        float duration = (float)slotAnimator.FrameCount / slotAnimator.FPS;
        yield return new WaitForSeconds(duration + 0.05f);

        animEntity.SetActive(false);
        slotAnimator.Stop(true);

        isAnimating = false;
        ApplyVisuals(currentValue);
    }

    private IEnumerator FlashGainAnimation(int newVal)
    {
        isAnimating = true;
        Image targetImage = (newVal == 2) ? FullSlotImage : HalfSlotImage;
        Entity targetEntity = (newVal == 2) ? fullSlotEntity : halfSlotEntity;

        if (targetEntity != null)
        {
            targetEntity.SetActive(true);
        }
        SetImageTint(targetImage, new Vector4(0.7f, 0.25f, 0.2f, 0.1f));
        yield return new WaitForSeconds(0.05f);
        SetImageTint(targetImage, new Vector4(0.8f, 0.3f, 0.25f, 0.5f));
        yield return new WaitForSeconds(0.05f);
        SetImageTint(targetImage, new Vector4(1, 0.8f, 0.75f, 0.9f));
        yield return new WaitForSeconds(0.05f);
        SetImageTint(targetImage, new Vector4(1.4f, 1.1f, 1.05f, 1));
        yield return new WaitForSeconds(0.06f);
        SetImageTint(targetImage, new Vector4(1, 1, 1, 1));

        isAnimating = false;
        ApplyVisuals(currentValue);
    }
}