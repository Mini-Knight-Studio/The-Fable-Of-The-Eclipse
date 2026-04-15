using System;
using Loopie;

class Spawner : Component
{
    public Entity entityToClone;
    public float distance;
    private Entity player;
    public bool alreadySpawned {  get; private set; }

    void OnCreate()
    {
        player = Entity.FindEntityByName("Player");
    }

    void OnUpdate()
    {
        if(!alreadySpawned && Vector3.Distance(transform.position, player.transform.position) <= distance)
        {
            alreadySpawned = true;
            Entity newClone = entityToClone.Clone(true);
            newClone.Name = newClone.Name.Replace("_Reference_", "");
            newClone.transform.position = transform.position;
            newClone.SetActive(true);
        }
    }

    public void ModifySpawnerStatus(bool state)
    {
        alreadySpawned = state;
    }

    void OnDrawGizmo()
    {
        if (!alreadySpawned)
        {
            Gizmo.DrawLine(transform.position + transform.Up, transform.position + transform.Up + (player.transform.position - transform.position).normalized*distance, Color.White);
        }
    }
};