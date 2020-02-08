using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject destroyPrefab;
    public int id;
    public int column;
    public int row;
    public bool isMatching;

    private int startX;
    private int startY;
    private GameObject otherBlock;
    private Board board;
    private Sounds soundManager;

    public static int counter = 30;
    public static int score = 0;

    void Start()
    {
        soundManager = FindObjectOfType<Sounds>();
        board = FindObjectOfType<Board>();
        column = (int)transform.position.x;
        row = (int)transform.position.y;
        startX = column;
        startY = row;
        isMatching = false;
    }

    private void Update()
    {

        if (Mathf.Abs(column - startX) > 0.1f)
        {
            Vector2 targetPos = new Vector2(column, transform.position.y);

            transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
        }

        if (Mathf.Abs(row - startY) > 0.1f)
        {
            Vector2 targetPos = new Vector2(transform.position.x, row);

            transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
        }

    }

    private void OnMouseUp()
    {
        CheckBlocks();
        if (isMatching)
        {
            if (counter <= 1)
            {
                counter = 0;
                board.GameOver();
            }
            else
            {
                counter--;

            }
        }

    }

    public IEnumerator SetPosition(int x, int y)
    {
        yield return new WaitForSeconds(0.1f);
        column = x;
        row = y;
    }

    public void CheckBlocks()
    {
        if (gameObject != null)
        {
            if (column >= 0 && column <= board.width - 1 && row >= 0 && row <= board.height - 1)
            {
                if (column != 0)
                {
                    GameObject leftBlock = board.allBlocks[column - 1, row];
                    if (leftBlock != null && id == leftBlock.GetComponent<Block>().id && !leftBlock.GetComponent<Block>().isMatching)
                    {
                        isMatching = true;
                        leftBlock.GetComponent<Block>().isMatching = true;
                        leftBlock.GetComponent<Block>().CheckBlocks();
                    }
                }

                if (column != board.width - 1)
                {
                    GameObject rightBlock = board.allBlocks[column + 1, row];
                    if (rightBlock != null && id == rightBlock.GetComponent<Block>().id && !rightBlock.GetComponent<Block>().isMatching)
                    {
                        isMatching = true;
                        rightBlock.GetComponent<Block>().isMatching = true;
                        rightBlock.GetComponent<Block>().CheckBlocks();
                    }
                }

                if (row != board.height - 1)
                {
                    GameObject upBlock = board.allBlocks[column, row + 1];
                    if (upBlock != null && id == upBlock.GetComponent<Block>().id && !upBlock.GetComponent<Block>().isMatching)
                    {
                        isMatching = true;
                        upBlock.GetComponent<Block>().isMatching = true;
                        upBlock.GetComponent<Block>().CheckBlocks();
                    }
                }

                if (row != 0)
                {
                    GameObject bottomBlock = board.allBlocks[column, row - 1];
                    if (bottomBlock != null && id == bottomBlock.GetComponent<Block>().id && !bottomBlock.GetComponent<Block>().isMatching)
                    {
                        isMatching = true;
                        bottomBlock.GetComponent<Block>().isMatching = true;
                        bottomBlock.GetComponent<Block>().CheckBlocks();
                    }
                }

            }
            StartCoroutine(DestroyBlocks());
        }
        else return;
    }

    IEnumerator DestroyBlocks()
    {
        soundManager.PlayDestroySound();
        score += 10;
        yield return new WaitForSeconds(0.1f);

        if (isMatching)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            GameObject destroyEffect = Instantiate(destroyPrefab, transform.position, Quaternion.Euler(0, 180, 0));
            Destroy(destroyEffect, 1f);
            Destroy(gameObject);
            board.allBlocks[column, row] = null;
            yield return StartCoroutine(board.CheckColsAndRows());
        }
        else
            board.allBlocks[column, row] = gameObject;
    }



}
