using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject deathScreenContainer;
    public GameObject winScreenContainer;
    public FirstPersonController player;
    private void Awake()
    {
        instance = this;
        if (deathScreenContainer != null ) deathScreenContainer.SetActive( false );
    }

    private void Start()
    {
        player = GetComponent<FirstPersonController>();
    }

    public void ShowDeathScreen()
    {
        player.Die();
        deathScreenContainer.SetActive( true );
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowWinScreen()
    {
        winScreenContainer.SetActive ( true );
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
