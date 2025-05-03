using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixController : MonoSingleton<MatrixController>
{
    public Vector2Int MatrixSize;
    public Transform GridContainer;
    public IngredientView currentView;

    public IngredientModel[,] ingredientGrid;
    public IngredientView ingredientGridPrefab;

    private Vector3 startPos, endPos;
    public Direction Dir;

    private Vector2Int offset;

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        None
    }

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

        this.ingredientGrid[0, 0].index = 1;
        this.ingredientGrid[0, 0].directions.Add(Direction.Left);
        this.ingredientGrid[0, 0].directions.Add(Direction.Right);
        this.ingredientGrid[0, 1].index = 1;
        this.ingredientGrid[0, 1].directions.Add(Direction.Left);
        this.ingredientGrid[0, 1].directions.Add(Direction.Right);

        this.ingredientGrid[1, 1].index = 2;
        this.ingredientGrid[1, 1].directions.Add(Direction.Up);
        this.ingredientGrid[1, 1].directions.Add(Direction.Down);
        this.ingredientGrid[2, 1].index = 2;
        this.ingredientGrid[2, 1].directions.Add(Direction.Up);
        this.ingredientGrid[2, 1].directions.Add(Direction.Down);

        this.ingredientGrid[2, 3].index = 3;
        this.ingredientGrid[2, 3].directions.Add(Direction.Left);

        this.ingredientGrid[2, 3].directions.Add(Direction.Right);
    }

    private void Update()
    {
        Direction dir = this.GetDirection();
        if (dir != Direction.None)
        {
            this.Dir = dir;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(dir + " " + currentView.name);
            Move(dir, currentView.poses);
        }
    }

    public Direction GetDirection()
    {
        Direction dir = Direction.None;
        if (Input.touchCount > 0)
        {
            Touch theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                startPos = theTouch.position;
            }
            else if (theTouch.phase == TouchPhase.Ended)
            {
                endPos = theTouch.position;

                float x = endPos.x - startPos.x;
                float y = endPos.y - startPos.y;

                if (Mathf.Abs(x) <= 0.1 && Mathf.Abs(y) <= 0.1)
                {
                    dir = Direction.None;
                }
                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    dir = x > 0 ? Direction.Right : Direction.Left;
                }
                else
                {
                    dir = y > 0 ? Direction.Up : Direction.Down;
                }
            }
        }
        return dir;
    }

    public void Move(Direction dir, List<Vector2Int> poses)
    {
        if (!ingredientGrid[poses[0].x, poses[0].y].directions.Contains(dir))
        {
            Debug.Log("Invalid move");
            return;
        }

        int id = ingredientGrid[poses[0].x, poses[0].y].index;
        List<Direction> ds = new List<Direction>(ingredientGrid[poses[0].x, poses[0].y].directions);

        for (int i = 0; i < poses.Count; i++)
        {
            ingredientGrid[poses[i].x, poses[i].y].index = 0;
            ingredientGrid[poses[i].x, poses[i].y].directions.Clear();
        }

        switch (dir)
        {
            case Direction.Left:
                offset = new Vector2Int(0, -1);
                break;
            case Direction.Right:
                offset = new Vector2Int(0, 1);
                break;
            case Direction.Up:
                offset = new Vector2Int(-1, 0);
                break;
            case Direction.Down:
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

            case Direction.Left:
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
            case Direction.Right:
                p = poses[poses.Count-1] + offset;
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
            case Direction.Up:
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
            case Direction.Down:
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
