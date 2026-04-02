using System;
using Loopie;

public class PlayerItems : Component
{
    public string item1Name;
    public string item2Name;

    private bool hasItem1;
    private bool hasItem2;

    public bool giveItem1;
    public bool giveItem2;

    public bool removeItem1;
    public bool removeItem2;

    void OnCreate()
    {

    }

    void OnUpdate()
    {
        if (giveItem1 && !hasItem1)
        {
            Entity.FindEntityByName(item1Name).SetActive(true);
            hasItem1 = true;
            removeItem1 = false;
        }
        else if (removeItem1 && hasItem1)
        {
            Entity.FindEntityByName(item1Name).SetActive(false);
            hasItem1 = false;
            giveItem1 = false;
        }

        if (giveItem2 && !hasItem2)
        {
            Entity.FindEntityByName(item2Name).SetActive(true);
            hasItem2 = true;
            removeItem2 = false;
        }
        else if (removeItem2 && hasItem2)
        {
            Entity.FindEntityByName(item2Name).SetActive(false);
            hasItem2 = false;
            giveItem2 = false;
        }
    }
};