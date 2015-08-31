using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Grid : MonoBehaviour
{
    public static int w = 10;
    public static int h = 15;
    public static Transform[,] grid = new Transform[w + 1, h + 1];
    public static Sequence mySequence;
    public GameMaster GameMaster;

    public AudioClip SingleRow;
    public AudioClip DoubleRow;
    public AudioClip TripleRow;
    public AudioClip QuadrupleRow;

    public AudioSource AudioSource;
    public GameObject BlueSplash;
    public GameObject GreenSplash;
    public GameObject RedSplash;
    public GameObject PurpleSplash;
    public GameObject LightBlueSplash;
    public GameObject YellowSplash;
    public GameObject OrangeSplash;

    void Update()
    {
        if (GameMaster.CanMove == false && GameMaster.GameOver == false && GameMaster.IsGameStarted == true)
        {
            deleteFullRows();
        }
    }

    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x/100),
                           Mathf.Round(v.y/100));
    }

    public static bool insideBorder(Vector2 pos)
    {
        if (Mathf.Round(pos.x/100) > 0 &&
            Mathf.Round(pos.x/100) < 11 &&
            Mathf.Round(pos.y/100) > 0)
        {
            return true;
        }

        return false;
    }

    public void deleteRow(int y)
    {
        for (int x = 1; x <= w; x++)
        {
            if (grid[x, y] != null)
            {
                ExplodeObject(x, y);
            }

            for (int i = y + 1; i <= h; i++)
            {
                if (grid[x, i] != null)
                {
                    grid[x, i].DOBlendableMoveBy(new Vector3(0, -100, 0), 0.05f, true).SetDelay(0.5f);
                    grid[x, i - 1] = grid[x, i];
                    grid[x, i] = null;
                }
            }

            
        }
    }

    void ExplodeObject(int x, int y)
    {
        if (grid[x, y].parent != null)
        {
            switch (grid[x, y].parent.name)
            {
                case "I(Clone)":
                    CreateSplash(LightBlueSplash, grid[x, y]);
                    break;
                case "Box(Clone)":
                    CreateSplash(BlueSplash, grid[x, y]);
                    break;
                case "T(Clone)":
                    CreateSplash(RedSplash, grid[x, y]);
                    break;
                case "L(Clone)":
                    CreateSplash(YellowSplash, grid[x, y]);
                    break;
                case "Inverted_L(Clone)":
                    CreateSplash(YellowSplash, grid[x, y]);
                    break;
                case "Z(Clone)":
                    CreateSplash(OrangeSplash, grid[x, y]);
                    break;
                case "Inverted_Z(Clone)":
                    CreateSplash(OrangeSplash, grid[x, y]);
                    break;
            }
        }

        Destroy(grid[x, y].gameObject);
        grid[x, y] = null;
    }

    void CreateSplash(GameObject Splash, Transform Position)
    {
        GameObject SplashEffect = (Instantiate(Splash, new Vector3(Position.position.x, Position.position.y, Position.position.z), Quaternion.identity) as GameObject);
        Destroy(SplashEffect, 0.5f);
    }

    public void ColoredBallTouch(int x, int y)
    {
        ExplodeObject(x, y);
        if (y == 0) y = 1;

        deleteRow(y - 1);
        PlaySound(1);
    }

    public static bool isRowFull(int y)
    {
        for (int x = 1; x <= w; ++x) { 
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    public static void ClearAll()
    {
        for(int y=0;y <= h; y++)
        {
            for (int x = 1; x <= w; x++)
            {
                if (grid[x, y] != null) { 
                    Destroy(grid[x, y].gameObject);
                    grid[x, y] = null;
                }
            }
        }
    }

    public void deleteFullRows()
    {
        int nDeletedRowsCount = 0;
        for (int y = 1; y < h; ++y)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                nDeletedRowsCount++;
                --y;

                GameMaster.Score += 100;
            }
        }

        if(nDeletedRowsCount > 0)
        {
            PlaySound(nDeletedRowsCount);
        }      
    }

    void PlaySound(int nRowCount)
    {
        switch (nRowCount)
        {
            case 1:
                AudioSource.PlayOneShot(SingleRow);
                break;
            case 2:
                AudioSource.PlayOneShot(DoubleRow);
                break;
            case 3:
                AudioSource.PlayOneShot(TripleRow);
                break;
            case 4:
                AudioSource.PlayOneShot(QuadrupleRow);
                break;
        }
    }

    public bool isFull(int x, int y)
    {
        return grid[x,y]!=null;
    }
}