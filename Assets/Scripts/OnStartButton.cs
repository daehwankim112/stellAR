using UnityEngine;
using UnityEngine.SceneManagement;

public class OnStartButton : MonoBehaviour
{
    public GameObject instructionPanel;
    public GameObject button;

    public void loadNewScene() {
        instructionPanel.SetActive(false);
        button.SetActive(false);
    }
}
