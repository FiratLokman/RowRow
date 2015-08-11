using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Grid : MonoBehaviour
{
    public static int w = 10;
    public static int h = 15;
    public static Transform[,] grid = new Transform[w+1, h+1];


    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),
                           Mathf.Round(v.y));
    }

    public static bool insideBorder(Vector2 pos)
    {
        //Debug.Log("Pos x :" + pos.x);
        //Debug.Log("Pos y :" + pos.y);
		if (Mathf.Round (pos.x) > 0 &&
			Mathf.Round (pos.x) < 11 &&
			Mathf.Round (pos.y) > 0) {
			return true;
		}

		return false;
    }

	public static void deleteRow(int y) {
		for (int x = 1; x <= w; ++x) {

            //grid[x, y].gameObject.AddComponent<Animator>();
            //var animator = grid[x, y].gameObject.GetComponent<Animator>();
            //animator.Play("breakup");

            //Grid gridwait = new Grid();
            //yield gridwait.WaitForAnimation(animator.GetComponent<Animation>());

            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
		}
	}

    public IEnumerator WaitForAnimation(Animation animation)
    { 
        do
        {
            yield return null;
        } while (animation.isPlaying);
    }

	public static void decreaseRow(int y) {
		for (int x = 1; x <= w; ++x) {
			if (grid[x, y] != null) {
				// Move one towards bottom
				grid[x, y-1] = grid[x, y];
				grid[x, y] = null;
				
				// Update Block position
				grid[x, y-1].position += new Vector3(0, -1, 0);
			}
		}
	}

	public static void decreaseRowsAbove(int y) {
		for (int i = y; i < h; ++i)
			decreaseRow(i);
	}

	public static bool isRowFull(int y) {
		for (int x = 1; x <= w; ++x)
			if (grid[x, y] == null)
				return false;
		return true;
	}

	public static void deleteFullRows() {
		for (int y = 1; y < h; ++y) {
			if (isRowFull(y)) {
				deleteRow(y);
				decreaseRowsAbove(y+1);
				--y;

				GameMaster.Score += 100;
			}
		}
	}
}