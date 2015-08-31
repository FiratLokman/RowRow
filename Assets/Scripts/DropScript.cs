using UnityEngine;
using System.Collections;
using System;

public class DropScript : MonoBehaviour {
	private Vector3 mousePos;
    public Grid grid;
    public AudioClip ColoredballSound;
    public AudioSource AudioSource;

    IEnumerator Start()
    {
        grid = FindObjectOfType<Grid>();
        AudioSource = FindObjectOfType<AudioSource>();
        
        while (isValidGridPos())
        {
            yield return new WaitForSeconds(1);
            
            transform.Translate(0, -100f, 0, Space.World);

            if (!isValidGridPos())
            {
                transform.Translate(0, 100f, 0, Space.World);
                GameMaster.CanMove = false;
                updateGrid();
                break;
            }

            if (transform.name == "ColoredBall(Clone)")
            {
                AudioSource.PlayOneShot(ColoredballSound);
            }

            updateGrid();
        }

        if (GameMaster.GameOver)
        {
            Destroy(gameObject);
        }

    }

    void Update()
    {
        if ((GameMaster.CanMove && !GameMaster.GameOver))
		{
            if (!GameMaster.IsPaused) { 
			    if(Input.GetMouseButtonDown(0))
			    {
				    mousePos = Input.mousePosition;
			    }

			    if(Input.GetMouseButtonUp(0))
			    {
				    Vector3 dist = Input.mousePosition - mousePos;

				    if(Vector3.Distance(Input.mousePosition, mousePos) <= 10 && transform.gameObject.name != "ColoredBall(Clone)")
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

						    transform.position += new Vector3(isLeft ? -100 : 100, 0, 0);
						
						    if (isValidGridPos()) {
							    updateGrid();
						    } else {
							    transform.position += new Vector3(isLeft ? 100 : -100, 0, 0);
						    }
					    } else if(dist.y < 50) {
                            if (Mathf.Abs(dist.y) > 160)
                            {
                                transform.position += new Vector3(0, -300f, 0);
                                if (isValidGridPos())
                                {
                                    updateGrid();
                                    if (transform.name == "ColoredBall(Clone)")
                                    {
                                        AudioSource.PlayOneShot(ColoredballSound);
                                    }
                                }
                                else
                                {
                                    transform.position += new Vector3(0, 300, 0);
                                }
                            }
                            else
                            {
                                transform.position += new Vector3(0, -100f, 0);
                                if (isValidGridPos())
                                {
                                    updateGrid();
                                    if (transform.name == "ColoredBall(Clone)")
                                    {
                                        AudioSource.PlayOneShot(ColoredballSound);
                                    }
                                }
                                else
                                {
                                    transform.position += new Vector3(0, 100, 0);
                                }
                            }
					    }
				    }
			    }

			    if (Input.GetKeyDown(KeyCode.RightArrow))
			    {
				    transform.position += new Vector3(100f, 0, 0);
				
				    if (isValidGridPos()) {
					    updateGrid();
				    } else {
					    transform.position += new Vector3(-100, 0, 0);
				    }
			    }
			    else if (Input.GetKeyDown(KeyCode.LeftArrow))
			    {
				    transform.position += new Vector3(-100f, 0, 0);
				    if (isValidGridPos())
				    {
					    updateGrid();
				    }
				    else
				    {
					    transform.position += new Vector3(100, 0, 0);
				    }
			    }
			    else if (Input.GetKeyDown(KeyCode.UpArrow) && transform.gameObject.name != "ColoredBall(Clone)") {
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
                    if (transform.gameObject.name == "ColoredBall(Clone)")
                    {
                        if(!grid.isFull((int)Grid.roundVec2(transform.position).x, (int)Grid.roundVec2(transform.position).y + 1))
                        {
                            transform.position += new Vector3(0, -100f, 0);
                            if (isValidGridPos())
                            {
                                if (transform.name == "ColoredBall(Clone)")
                                {
                                    AudioSource.PlayOneShot(ColoredballSound);
                                }
                                updateGrid();
                            }
                            else
                            {
                                transform.position += new Vector3(0, 100, 0);
                            }
                        }
                    }
                    else
                    {
                        transform.position += new Vector3(0, -100f, 0);
                        if (isValidGridPos())
                        {
                            updateGrid();
                        }
                        else
                        {
                            transform.position += new Vector3(0, 100, 0);
                        }
                    }

				    
			    }
            }
        }
        else
        {
            // ColoredBall touched!
            if (transform.gameObject.name == "ColoredBall(Clone)")
            {
                updateGrid();
                Vector2 ColoredBallPosition = Grid.roundVec2(transform.position);
                grid.ColoredBallTouch((int)ColoredBallPosition.x, (int)ColoredBallPosition.y);
            }

            FindObjectOfType<Spawner>().spawnNext();
            GameMaster.CanMove = true;
            enabled = false;
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

            //if any object in new object creation spot, game over
			if (Grid.grid[(int)v.x, (int)v.y] != null && Grid.grid[(int)v.x, (int)v.y].parent != transform){
                if (Math.Round(transform.position.x/100) == 5 && Math.Round(transform.position.y/100) == 14)
                {
                    GameMaster.GameOver = true;
                    enabled = false;
                }
                return false;
            }
        }

        return true;
    }
}