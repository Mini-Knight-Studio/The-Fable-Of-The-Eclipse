using Loopie;
using System;
using System.Collections.Generic;
using System.Text;

class GODModeSpawner : Component
{
    class EnemyDirection
    {
        public string EnemyName;
        public Entity ReferenceEntity;
    }
    private Text EnemyNameText;
    private List<EnemyDirection> EnemyDirections;
    private int currentListIndex = 0;
    private RaycastHit hit;
    private Entity camera;
    private int LayerMask;
    void OnCreate()
    {
        EnemyDirections = new List<EnemyDirection>();
        EnemyNameText = Entity.FindEntityByName("EnemyName").GetComponent<Text>();
        Entity enemies = Entity.FindEntityByName("Enemies");
        foreach (Entity child in enemies.GetChildren())
        {
            if (!child.Name.Contains("_Reference"))
                continue;
            EnemyDirection newDirection = new EnemyDirection();
            string cleanName = FormatName(child.Name);
            newDirection.EnemyName = cleanName;
            newDirection.ReferenceEntity = child;
            EnemyDirections.Add(newDirection);
        }
        currentListIndex = 0;
        camera = Entity.FindEntityByName("PlayerCamera");
        EnemyNameText.SetText($"{EnemyDirections[currentListIndex].EnemyName}");
        int Floor = Collisions.GetLayerBit("Floor");
        int Walls = Collisions.GetLayerBit("WorldLimits");
        LayerMask = Floor | Walls;
    }

    void KillAllEnemies()
    {
        Entity enemies = Entity.FindEntityByName("Enemies");
        foreach (Entity child in enemies.GetChildren())
        {
            if (child.Name.Contains("_Reference"))
                continue;
            child.Destroy();
        }
    }

    string FormatName(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        const string suffix = "_Reference";

        if (input.EndsWith(suffix))
            input = input.Substring(0, input.Length - suffix.Length);

        StringBuilder result = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (i > 0
                && char.IsUpper(c)
                && !char.IsUpper(input[i - 1]))
            {
                result.Append(' ');
            }

            result.Append(c);
        }
        return result.ToString();
    }

    public void ChangeIndex()
    {
        currentListIndex++;
        if (currentListIndex == EnemyDirections.Count)
            currentListIndex = 0;
        EnemyNameText.SetText($"{EnemyDirections[currentListIndex].EnemyName}");
    }

    void OnUpdate()
    {
        if (Input.IsMouseButtonDown(MouseButton.MOUSE_RIGHT) && Collisions.Raycast(camera.transform.position, camera.transform.Forward, 1000.0f, out hit, LayerMask))
        {
            if (hit.collider.Layer != "Floor")
            {
                return;
            }
            Entity new_enemy = EnemyDirections[currentListIndex].ReferenceEntity.Clone(true);
            int angles = Loopie.Random.Range(0, 360);
            new_enemy.transform.position = hit.point;
            new_enemy.transform.Rotate(new Vector3(0, angles, 0), Transform.Space.LocalSpace);
            new_enemy.Name = EnemyDirections[currentListIndex].EnemyName;
            new_enemy.SetActive(true);
        }

        if(Input.IsKeyDown(KeyCode.O))
        {
            KillAllEnemies();
        }
    }
};