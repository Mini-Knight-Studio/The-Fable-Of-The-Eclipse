using System;
using Loopie;

public abstract class PlayerComponent : Component
{
    protected Player player;

    public void SetOwner(Player player)
    {
        this.player = player;
    }

    public Player GetOwner()
    {
        return player;
    }
};