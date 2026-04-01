using System;
using Loopie;

class Load : Component
{
    bool isLoading = false;

    public void LoadPreviousSave()
    {
        isLoading = true;
        Debug.Log("Load Success");
    }
};