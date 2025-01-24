using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public TextMeshPro survivedLevelsText;
    public int levelCount;

    // Start is called before the first frame update
    void Start()
    {
        survivedLevelsText.text = "You survived Till...  Level: " + GameManager.instance.level;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GameManager.instance.ResetScore();
            //TimerScript.instance.ResetTimer();
            SceneManager.LoadScene("MainScene");
        }
    }
}
