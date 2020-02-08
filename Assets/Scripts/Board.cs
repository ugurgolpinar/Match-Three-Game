using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    public GameObject[] blocks;
    public GameObject[,] allBlocks;
    public GameObject tile;
    public GameObject gameOverPanel;
    public int width = 8;
    public int height = 8;

    private GameObject candysParent;
    private GameObject tilesParent;
    public bool gameOver;

    void Start()
    {
        allBlocks = new GameObject[width, height];
        candysParent = GameObject.Find("Candys");
        tilesParent = GameObject.Find("Tiles");
        SpawnBlocks();
    }

    private void SpawnBlocks()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int index = Random.Range(0, blocks.Length);
                Vector2 tempPos = new Vector2(i, j);
                GameObject tiles = Instantiate(tile, tempPos, Quaternion.identity) as GameObject;
                tiles.transform.parent = tilesParent.transform;
                GameObject block = Instantiate(blocks[index], tempPos, Quaternion.identity) as GameObject;
                block.transform.parent = candysParent.transform;
                allBlocks[i, j] = block;
            }
        }
    }

    public IEnumerator CheckColsAndRows()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allBlocks[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allBlocks[i, j].GetComponent<Block>().row -= nullCount;
                    allBlocks[i, j - nullCount] = allBlocks[i, j];
                    allBlocks[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return StartCoroutine(FillBoard());
    }

    IEnumerator FillBoard()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allBlocks[i, j] == null)
                {
                    int index = Random.Range(0, blocks.Length);
                    Vector3 tempPos = new Vector3(i, j + 7);
                    GameObject block = Instantiate(blocks[index], tempPos, Quaternion.identity) as GameObject;
                    StartCoroutine(block.GetComponent<Block>().SetPosition(i, j));
                    block.transform.parent = candysParent.transform;
                    allBlocks[i, j] = block;
                    if (gameOver)
                    {
                        block.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
            }
        }
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
        FindObjectOfType<Sounds>().PlayGameOverSound();
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks)
        {
            block.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void RestartGame()
    {
        Block.counter = 30;
        Block.score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
