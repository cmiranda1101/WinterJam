using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ButtonFunctions : MonoBehaviour
{
    public void onResume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.instance.pauseMenu.SetActive(false);
        GameManager.instance.menueQueue.Pop();
        GameManager.instance.HUD.SetActive(true);
        GameManager.instance.isPaused = false;
        GameManager.instance.playerScript.enabled = true;
        Time.timeScale = 1f;
    }

    public void onSettings()
    {
        GameManager.instance.menueQueue.Peek().SetActive(false);
        GameManager.instance.menueQueue.Push(GameManager.instance.SettingsMenu);
        GameManager.instance.menueQueue.Peek().SetActive(true);
    }

    public void onBack()
    {
        GameManager.instance.menueQueue.Peek().SetActive(false);
        GameManager.instance.menueQueue.Pop();
        GameManager.instance.menueQueue.Peek().SetActive(true);
    }

    public void onControls()
    {
        GameManager.instance.menueQueue.Peek().SetActive(false);
        GameManager.instance.menueQueue.Push(GameManager.instance.ControlsMenu);
        GameManager.instance.menueQueue.Peek().SetActive(true);
    }

    public void onExit()
    {

    }
}
