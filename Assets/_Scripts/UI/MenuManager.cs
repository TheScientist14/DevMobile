using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private MainMenuBehaviour m_MainMenu;
	[SerializeField] private PauseMenuBehaviour m_PauseMenu;
	[SerializeField] private SettingsMenuBehaviour m_SettingsMenu;
	[SerializeField] private DeathMenuBehaviour m_DeathMenu;

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}
}
