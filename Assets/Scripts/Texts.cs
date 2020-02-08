using UnityEngine;
using UnityEngine.UI;

public class Texts : MonoBehaviour
{
    public Text scoreText;
    public Text countText;
    private GameObject board;

    void Start()
    {
        board = GameObject.Find("Board");
    }

    void Update()
    {
        scoreText.text = "Score: " + Block.score;
        countText.text = "Count: " + Block.counter;
        if (board.GetComponent<Board>().gameOver)
        {
            scoreText.GetComponent<Animator>().SetTrigger("GameOver");
            countText.enabled = false;
        }
    }
}
