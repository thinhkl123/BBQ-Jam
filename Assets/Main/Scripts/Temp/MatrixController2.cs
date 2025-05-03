using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixController2 : MonoSingleton<MatrixController2>
{
    public Vector2Int MatrixSize;
    public Transform GridContainer;
    public IngredientView currentView;

    public IngredientModel[,] ingredientGrid;

    private Vector3 startPos, endPos;
    public MatrixController.Direction Dir;

    private Vector2Int offset;

    protected override void Awake()
    {
        this.ingredientGrid = new IngredientModel[8, 6];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                this.ingredientGrid[i, j] = new IngredientModel();
                this.ingredientGrid[i, j].Init();
            }
        }

        // Level 1
        this.ingredientGrid[0, 0].index = 1;
        this.ingredientGrid[0, 0].directions.Add(MatrixController.Direction.Left);
        this.ingredientGrid[0, 0].directions.Add(MatrixController.Direction.Right);
        this.ingredientGrid[0, 1].index = 1;
        this.ingredientGrid[0, 1].directions.Add(MatrixController.Direction.Left);
        this.ingredientGrid[0, 1].directions.Add(MatrixController.Direction.Right);

        this.ingredientGrid[4, 3].index = 2;
        this.ingredientGrid[4, 3].directions.Add(MatrixController.Direction.Left);
        this.ingredientGrid[4, 3].directions.Add(MatrixController.Direction.Right);
        this.ingredientGrid[4, 4].index = 2;
        this.ingredientGrid[4, 4].directions.Add(MatrixController.Direction.Left);
        this.ingredientGrid[4, 4].directions.Add(MatrixController.Direction.Right);

        this.ingredientGrid[3, 0].index = 3;
        this.ingredientGrid[3, 0].directions.Add(MatrixController.Direction.Up);
        this.ingredientGrid[3, 0].directions.Add(MatrixController.Direction.Down);
        this.ingredientGrid[4, 0].index = 3;
        this.ingredientGrid[4, 0].directions.Add(MatrixController.Direction.Up);
        this.ingredientGrid[4, 0].directions.Add(MatrixController.Direction.Down);

        this.ingredientGrid[0, 4].index = 4;
        this.ingredientGrid[0, 4].directions.Add(MatrixController.Direction.Up);
        this.ingredientGrid[0, 4].directions.Add(MatrixController.Direction.Down);
        this.ingredientGrid[1, 4].index = 4;
        this.ingredientGrid[1, 4].directions.Add(MatrixController.Direction.Up);
        this.ingredientGrid[1, 4].directions.Add(MatrixController.Direction.Down);
    }

    private void Update()
    {
        MatrixController.Direction dir = this.GetDirection();
        if (dir != MatrixController.Direction.None)
        {
            this.Dir = dir;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(this.Dir + " " + currentView.name);
            Move(Dir, currentView.poses);
        }
    }

    private MatrixController.Direction GetDirection()
    {
        MatrixController.Direction dir = MatrixController.Direction.None;
        if (Input.touchCount > 0)
        {
            Touch theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                startPos = theTouch.position;
            }
            else if (theTouch.phase == TouchPhase.Ended /*|| theTouch.phase == TouchPhase.Moved*/)
            {
                endPos = theTouch.position;

                float x = endPos.x - startPos.x;
                float y = endPos.y - startPos.y;

                if (Mathf.Abs(x) <= 0.1 && Mathf.Abs(y) <= 0.1)
                {
                    dir = MatrixController.Direction.None;
                }
                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    dir = x > 0 ? MatrixController.Direction.Right : MatrixController.Direction.Left;
                }
                else
                {
                    dir = y > 0 ? MatrixController.Direction.Up : MatrixController.Direction.Down;
                }
            }
        }
        return dir;
    }

    public void Move(MatrixController.Direction dir, List<Vector2Int> poses)
    {
        if (!ingredientGrid[poses[0].x, poses[0].y].directions.Contains(dir))
        {
            Debug.Log("Invalid move");
            return;
        }

        int id = ingredientGrid[poses[0].x, poses[0].y].index;
        List<MatrixController.Direction> ds = new List<MatrixController.Direction>(ingredientGrid[poses[0].x, poses[0].y].directions);

        for (int i = 0; i < poses.Count; i++)
        {
            ingredientGrid[poses[i].x, poses[i].y].index = 0;
            ingredientGrid[poses[i].x, poses[i].y].directions.Clear();
        }

        switch (dir)
        {
            case MatrixController.Direction.Left:
                offset = new Vector2Int(0, -1);
                break;
            case MatrixController.Direction.Right:
                offset = new Vector2Int(0, 1);
                break;
            case MatrixController.Direction.Up:
                offset = new Vector2Int(-1, 0);
                break;
            case MatrixController.Direction.Down:
                offset = new Vector2Int(1, 0);
                break;
        }

        List<Vector2Int> newPosses;
        bool check = true;
        while (check)
        {
            newPosses = new List<Vector2Int>();
            for (int i = 0; i < poses.Count; i++)
            {
                Vector2Int newPos = poses[i] + offset;
                if (!IsInMatrix(newPos) || ingredientGrid[newPos.x, newPos.y].index != 0)
                {
                    check = false;
                    break;
                }
                else
                {
                    //Debug.Log(newPos);
                    newPosses.Add(newPos);
                }
            }

            if (!check) break;
            poses = newPosses;
        }

        switch (dir)
        {

            case MatrixController.Direction.Left:
                Vector2Int p = poses[0] + offset;
                if (!IsInMatrix(p))
                {
                    if (currentView.isCooked)
                    {
                        Debug.Log("Cut ra");
                        QueueController.Instance.AddQueue(currentView.FoodType);
                        Destroy(currentView.gameObject);
                        return;
                    }
                }
                else if (ingredientGrid[p.x, p.y].index != 0)
                {
                    Debug.Log("Touch");
                    currentView.SetCook();
                }

                break;
            case MatrixController.Direction.Right:
                p = poses[poses.Count - 1] + offset;
                if (!IsInMatrix(p))
                {
                    if (currentView.isCooked)
                    {
                        Debug.Log("Cut ra");
                        QueueController.Instance.AddQueue(currentView.FoodType);
                        Destroy(currentView.gameObject);
                        return;
                    }
                }
                else if (ingredientGrid[p.x, p.y].index != 0)
                {
                    Debug.Log("Touch");
                    currentView.SetCook();
                }

                break;
            case MatrixController.Direction.Up:
                p = poses[0] + offset;
                if (!IsInMatrix(p))
                {
                    if (currentView.isCooked)
                    {
                        Debug.Log("Cut ra");
                        QueueController.Instance.AddQueue(currentView.FoodType);
                        Destroy(currentView.gameObject);
                        return;
                    }
                }
                else if (ingredientGrid[p.x, p.y].index != 0)
                {
                    Debug.Log("Touch");
                    currentView.SetCook();
                }

                break;
            case MatrixController.Direction.Down:
                p = poses[poses.Count - 1] + offset;
                if (!IsInMatrix(p))
                {
                    if (currentView.isCooked)
                    {
                        Debug.Log("Cut ra");
                        QueueController.Instance.AddQueue(currentView.FoodType);
                        Destroy(currentView.gameObject);
                        return;
                    }
                }
                else if (ingredientGrid[p.x, p.y].index != 0)
                {
                    Debug.Log("Touch");
                    currentView.SetCook();
                }

                break;
        }

        for (int i = 0; i < poses.Count; i++)
        {
            ingredientGrid[poses[i].x, poses[i].y].index = id;
            ingredientGrid[poses[i].x, poses[i].y].directions = ds;
        }


        currentView.SetPoses(poses);

        return;
    }

    //public bool isContains()
    //{

    //}

    private bool IsInMatrix(Vector2Int pos)
    {
        return (0 <= pos.x && pos.x < MatrixSize.x && 0 <= pos.y && pos.y < MatrixSize.y);
    }

    public Vector3 GetPosContainer(Vector2Int pos)
    {
        return GridContainer.GetChild(pos.x * MatrixSize.y + pos.y).position;
    }

    public Vector3 GetPosition(List<Vector2Int> positions)
    {
        if (positions.Count == 1) return GetPosContainer(positions[0]);

        Vector3 pos1 = GetPosContainer(positions[0]);
        Vector3 pos2 = GetPosContainer(positions[1]);

        return (pos1 + pos2) / 2;
    }
}
