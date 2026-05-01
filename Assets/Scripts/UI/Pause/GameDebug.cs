using System;
using Loopie;

public class GameDebug : Component
{
    public Entity debugMenuEntity;
    public Entity pauseMenuEntity;
    private PauseMenu pauseMenuScript;

    public bool debugMenuActive = false;

    public bool pauseTransparencyActive = false;
    
    void OnCreate()
    {
        if (pauseMenuEntity != null)
        {
            pauseMenuScript = pauseMenuEntity.GetComponent<PauseMenu>();
        }
        else
        {
            Debug.Log("Error: There is no passPageEntity Entity assigned.");
        }
    }
    void OnUpdate()
    {
    }
    public void ToggleDebugMenu() 
    {
        debugMenuActive = !debugMenuActive;
        debugMenuEntity.SetActive(debugMenuActive);
    }
    public void TogglePauseTransparency() 
    {
        pauseTransparencyActive = !pauseTransparencyActive;
        if (pauseTransparencyActive)
        {
            Vector4 lowOpacityColor = new Vector4(1, 1, 1, 0.25f);
            pauseMenuScript.backgroundImage.SetTint(lowOpacityColor);
            pauseMenuScript.ilustrationImage.SetTint(lowOpacityColor);
        }
        else
        {
            Vector4 fullOpacityColor = new Vector4(1, 1, 1, 1);
            pauseMenuScript.backgroundImage.SetTint(fullOpacityColor);
            pauseMenuScript.ilustrationImage.SetTint(fullOpacityColor);
        }
    }
};