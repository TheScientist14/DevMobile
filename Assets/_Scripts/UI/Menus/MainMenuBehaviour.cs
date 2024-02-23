using UnityEngine;

public class MainMenuBehaviour : BasicPanel<MenuGameState>
{
    public void Quit()
    {
        Application.Quit();
    }
}
