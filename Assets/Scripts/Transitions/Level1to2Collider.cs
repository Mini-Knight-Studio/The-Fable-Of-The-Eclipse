using System;
using Loopie;

class Level1to2Collider : SceneTransition
{
    public string SceneUUID;
    private BoxCollider collision;

    void OnCreate()
    {
        collision = entity.GetComponent<BoxCollider>();
        collision.Trigger = true;
        UUID = SceneUUID;
    }

    void OnUpdate()
    {
        if (collision.HasCollided)
        {
            if (GlobalDatabase.Data.Puzzles.Puzzle1Completed)
            {
                StartTransition();
            }
        }
    }
};