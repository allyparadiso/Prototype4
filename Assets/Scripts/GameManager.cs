using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshPro youWin;

    private void Start()
    {
        youWin.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("End"))
        {
            youWin.gameObject.SetActive(true);
        }
        else
        {
            youWin.gameObject.SetActive(false);
        }
    }

    public void Die()
    {

    }
}
