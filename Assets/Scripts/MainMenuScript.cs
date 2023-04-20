using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] string _nameOfGameScene;
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _controls;
    [SerializeField] GameObject _credits;
    [SerializeField] GameObject _backButton;

    public void StartGame()
    {
        SceneManager.LoadScene(_nameOfGameScene);
    }

    public void Controls()
    {
        _controls.SetActive(true);
        _backButton.SetActive(true);
        _mainMenu.SetActive(false);
    }

    public void Credits()
    {
        _credits.SetActive(true);
        _backButton.SetActive(true);
        _mainMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowMainMenu()
    {
        _mainMenu.SetActive(true);
        _controls.SetActive(false);
        _credits.SetActive(false);
        _backButton.SetActive(false);
    }
}
