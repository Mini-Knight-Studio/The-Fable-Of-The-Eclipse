using System;
using Loopie;

class MenuShower : Component
{
    public string MenuEntity;
    public int Key;
    public bool ActiveOnStart;
    private Entity Menu;

    void OnCreate()
    {
        Menu = Entity.FindEntityByName(MenuEntity);
        Menu.SetActive(ActiveOnStart);
    }

    void OnUpdate()
    {
        if (Input.IsKeyDown((KeyCode)Key))
        {
            Menu.SetActive(!Menu.Active);
        }
    }
};
