using System;
using Loopie;

class BossEntry : Component
{
    public float time = 3f;
    public float transformAmount = 0.5f;
    public float rotationAmount = 0.5f;

    private string cameraName = "PlayerCamera";

    private PlayerCamera camera;
    private BoxCollider collider;

    void OnCreate()
    {
        Entity cameraEntity = Entity.FindEntityByName(cameraName);
        if (cameraEntity != null)
        {
            camera = cameraEntity.GetComponent<PlayerCamera>();
        }
        else
        {
            Debug.Log("There is no PlayerCamera");
        }

        collider = entity.GetComponent<BoxCollider>();
        if (collider == null)
        {
            Debug.Log("The VistaPoint lacks a Collider");
        }
    }

    void OnUpdate()
    {
        if (camera == null) return;
        if (collider == null) return;

        if (collider.HasCollided)
        {
            camera.SetIsShaking(true, time, transformAmount, rotationAmount);
        }
    }
};