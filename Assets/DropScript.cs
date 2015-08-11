using UnityEngine;
using System.Collections;
using System;

public class DropScript : MonoBehaviour {
    private bool Move = true;
	private Vector3 mousePos;

    IEnumerator Start() {
        while (isValidGridPos()) {
            yield return new WaitForSeconds(1);
            transform.Translate(0, -1f, 0, Space.World);

			if(!isValidGridPos()){
				if(transform.position.x == FindObjectOfType<Spawner>().transform.position.x &&
				   transform.position.y == FindObjectOfType<Spawner>().transform.position.y)
				{
					GameMaster.GameOver = true;
					Debug.Log ("Game Over");
					break;
				}
				transform.Translate(0, 1f, 0, Space.World);
				updateGrid();
				break;
			}

			updateGrid();
        }


        Move = false;
    }

    void Update()
    {
        if (Move && !GameMaster.GameOver)
		{
			if(Input.GetMouseButtonDown(0))
			{
				mousePos = Input.mousePosition;
			}

			if(Input.GetMouseButtonUp(0))
			{
				Vector3 dist = Input.mousePosition - mousePos;

				if(Vector3.Distance(Input.mousePosition, mousePos) <= 10)
				{
					transform.Rotate(0, 0, -90);
					if (isValidGridPos())
					{
						updateGrid();
					}
					else
					{
						transform.Rotate(0, 0, 90);
					}
				} else {
					if(Mathf.Abs(dist.x) > Mathf.Abs(dist.y))
					{
						bool isLeft = dist.x < 0;

						transform.position += new Vector3(isLeft ? -1 : 1, 0, 0);
						
						if (isValidGridPos()) {
							updateGrid();
						} else {
							transform.position += new Vector3(isLeft ? 1 : -1, 0, 0);
						}
					} else if(dist.y < 0) {
                        Debug.Log(Mathf.Abs(dist.y));
                        if (Mathf.Abs(dist.y) > 120)
                        {
                            transform.position += new Vector3(0, -3f, 0);
                            if (isValidGridPos())
                            {
                                updateGrid();
                            }
                            else
                            {
                                transform.position += new Vector3(0, 3, 0);
                            }
                        }
                        else
                        {
                            transform.position += new Vector3(0, -1f, 0);
                            if (isValidGridPos())
                            {
                                updateGrid();
                            }
                            else
                            {
                                transform.position += new Vector3(0, 1, 0);
                            }
                        }
					}
				}
			}

			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				transform.position += new Vector3(1f, 0, 0);
				
				if (isValidGridPos()) {
					updateGrid();
				} else {
					transform.position += new Vector3(-1, 0, 0);
				}
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				transform.position += new Vector3(-1f, 0, 0);
				if (isValidGridPos())
				{
					updateGrid();
				}
				else
				{
					transform.position += new Vector3(1, 0, 0);
				}
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow)) {
				transform.Rotate(0, 0, -90);
				if (isValidGridPos())
				{
					updateGrid();
				}
				else
				{
					transform.Rotate(0, 0, 90);
				}
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				transform.position += new Vector3(0, -1f, 0);
				if (isValidGridPos())
				{
					updateGrid();
				}
				else
				{
					transform.position += new Vector3(0, 1, 0);
				}
			}

        }
        else
        {
			Grid.deleteFullRows();
            // Disable script
            enabled = false;

			FindObjectOfType<Spawner>().spawnNext();
        }
    }

    void updateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y <= Grid.h; ++y)
			for (int x = 0; x <= Grid.w; ++x)
				if (Grid.grid [x, y] != null)
				if (Grid.grid [x, y].parent == transform) {
					Grid.grid [x, y] = null;
				}
        // Add new children to grid
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.roundVec2(child.position);

            //Debug.Log("x :" + (int)v.x);
            //Debug.Log("y :" + (int)v.y);

            Grid.grid[(int)v.x, (int)v.y] = child;

            //Debug.Log("Child position added x,y : " + (int)v.x + "," + (int)v.y  + " : " + child.transform.position.x + "," + child.transform.position.y);

        }
    }

    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            if (!Grid.insideBorder(child.position))
            {
                return false;
            }

			Vector2 v = Grid.roundVec2(child.position);
            
			//Debug.Log("BURASI :" + (int)v.x + "," + (int)v.y);
			if (Grid.grid[(int)v.x, (int)v.y] != null && Grid.grid[(int)v.x, (int)v.y].parent != transform){
				return false;
            }
        }

        return true;
    }
}