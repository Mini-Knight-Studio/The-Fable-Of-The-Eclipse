using System;
using Loopie;

class ColliderSceneTransition : SceneTransition
{
    [Header("Transition Fade Reference")]
    public Entity levelFadeInOut;
    private FadeInOutEvent levelFadeInOutEvent;

    [Header("Scene to go to")]
    public string SceneUUID;
    private BoxCollider collision;

    void OnCreate()
    {
        collision = entity.GetComponent<BoxCollider>();
        collision.Trigger = true;
        UUID = SceneUUID;

        levelFadeInOutEvent = levelFadeInOut.GetComponent<FadeInOutEvent>();
        levelFadeInOutEvent.OnFadeInComplete += GoToScene;
    }

    void OnUpdate()
    {
        if(collision.HasCollided)
        {
            levelFadeInOutEvent.StartFade();
        }
    }

    private void GoToScene()
    {
        DatabaseRegistry.playerDB.Player.SetCurrentScene(UUID);
        StartTransition();
    }

    void OnDestroy()
    {
        if (levelFadeInOutEvent == null)
            return;
        levelFadeInOutEvent.OnFadeInComplete -= GoToScene;
    }
};
