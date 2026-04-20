using System;
using Loopie;

class Spawner : Component
{
    public int spawnerID;

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


    /// YOU MUST PASS THE CORRECT ENTITY REFERENCE BASED ON THE ENEMY TYPE (GOLEM, OR BLOB -> TAKE CARE OF THE TYPE OF THE SLIME)
    public static void Spawn(Entity entityToSpawn, Vector3 position)
    {
        Entity newClone = entityToSpawn.Clone(true);
        newClone.Name = newClone.Name.Replace("_Reference_", "");
        newClone.transform.position = position;
        newClone.SetActive(true);
    }
};