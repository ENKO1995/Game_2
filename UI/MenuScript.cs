using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private GameObject creditsMenu;

    private bool pauseActive;
    private bool optionsActive;
    private bool creditsActive;

    public void Start()
    {
        pauseActive = false;
        SetMenuActive(pauseMenu, pauseActive);
        optionsActive = false;
        SetMenuActive(optionsMenu, optionsActive);
        creditsActive = false;
        SetMenuActive(creditsMenu, creditsActive);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuToogle();
        }
    }

    private void SetMenuActive(GameObject _menu, bool _active)
    {
        if (_menu != null)
        {
            _menu.SetActive(_active);
        }
    }

    public void PauseMenuToogle()
    {
        pauseActive = !pauseActive;
        SetMenuActive(pauseMenu, pauseActive);
        Time.timeScale = pauseActive == true ? 0 : 1;

        if (pauseActive == true)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    public void Options()
    {
        if (creditsActive == false)
        {
            optionsActive = !optionsActive;
            SetMenuActive(optionsMenu, optionsActive);
        }
    }

    public void Credits()
    {
        if (optionsActive == false)
        {
            creditsActive = !creditsActive;
            SetMenuActive(creditsMenu, creditsActive);
        }
    }

    public void GoToHub()
    {
        if (pauseActive == true)
        {
            pauseActive = !pauseActive;
        }        
        Time.timeScale = pauseActive == true ? 0 : 1;
        SceneManager.LoadScene(sceneName:"Level_Dennis");
        Cursor.visible = false;
    }

    public void BacktoMenu()
    {
        pauseActive = !pauseActive;
        Time.timeScale = pauseActive == true ? 1 : 0;
        SceneManager.LoadScene(sceneName: "MainMenu");
        Cursor.visible = true;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
