using LevelManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodLevelData;
using MatrixData;
using DG.Tweening;
using System;

public class MatrixController : MonoSingleton<MatrixController>
{
    public Vector2Int MatrixSize;
    public Transform GridContainer;
    public IngredientView currentView;

    public IngredientModel[,] ingredientGrid;

    private Vector3 startPos, endPos;
    public Direction Dir;

    private Vector2Int offset;

    public bool isPressing = false;
    private bool isTutorial = false;
    private bool isSetCookedSuccess = false;
    private bool isSetCookedFailure = false;

    //Port
    private bool isPort = false;
    private List<Vector2Int> newPosses;
    private List<Direction> newDs;
    public float TimeWait = 0f;
    public float TouchTime = 0f;

    public Vector3 firstPos;


    private void Start()
    {
        isPressing = false;

        SpawnLevel(GameManager.Instance.currentLevel - 1);
    }

    private void SpawnLevel(int level)
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

        isTutorial = false;

        if (DataManager.Instance.LevelData.Levels[level].TutorialId != 0)
        {
            isTutorial = true;
            SpawnTutorialGuide(DataManager.Instance.LevelData.Levels[level].TutorialId);
        }

        SetMatrixSize(DataManager.Instance.FoodLevelData.FoodsLevelList[level].MatrixType);

        SpawnMatrix(DataManager.Instance.FoodLevelData.FoodsLevelList[level].MatrixType);

        SpawnFoods(DataManager.Instance.FoodLevelData.FoodsLevelList[level].FoodLevelList);

        if (DataManager.Instance.LevelData.Levels[level].IceId != 0)
        {
            SpawnIce(DataManager.Instance.IceData.IceLevelList[DataManager.Instance.LevelData.Levels[level].IceId - 1]);
        }

        if (DataManager.Instance.LevelData.Levels[level].PortId != 0)
        {
            SpawnPort(DataManager.Instance.PortLevelData.PortLevels[DataManager.Instance.LevelData.Levels[level].PortId - 1]);
        }
    }

    private void SpawnPort(PortLevelConfig portLevel)
    {
        foreach (var portData in portLevel.Ports)
        {
            List<Vector2Int> l = new List<Vector2Int>();
            l.Add(portData.Pos);
            Vector3 pos = GetPosition(l);
            pos.y = 1.41f;

            PortView portGO = Instantiate(DataManager.Instance.PortData.GetPrefab(portData.PortType));
            portGO.transform.position = pos;
            portGO.transform.SetParent(this.transform);

            this.ingredientGrid[portData.Pos.x, portData.Pos.y].index = 5000;
            this.ingredientGrid[portData.Pos.x, portData.Pos.y].portView = portGO;
        }
    }

    private void SpawnIce(IceLevel iceLevel)
    {
        foreach (var ice in iceLevel.IceList)
        {
            List<Vector2Int> l = new List<Vector2Int>();
            l.Add(ice.Pos);
            Vector3 pos = GetPosition(l);
            pos.y = 1.41f;
            IceView iceGO = Instantiate(DataManager.Instance.IceData.IcePrefab, pos, DataManager.Instance.IceData.IcePrefab.transform.rotation, this.transform);
            iceGO.SetHealth(ice.Health);

            this.ingredientGrid[ice.Pos.x, ice.Pos.y].index = -ice.Health;
            this.ingredientGrid[ice.Pos.x, ice.Pos.y].IceView = iceGO;
        }
    }

    private void SpawnTutorialGuide(int tutorialId)
    {
        GameObject GO = Instantiate(Resources.Load<GameObject>("Tutorial " + tutorialId.ToString()));
        GO.name = "Tutorial " + tutorialId.ToString();
    }

    private void SetMatrixSize(MatrixType type)
    {
        this.MatrixSize = DataManager.Instance.MatrixData.GetMatrixSize(type);
    }

    private void SpawnMatrix(MatrixType matrixType)
    {
        GameObject matrixGO = Instantiate(DataManager.Instance.MatrixData.GetMatrixPrefab(matrixType), DataManager.Instance.MatrixData.GetMatrixPosition(matrixType), Quaternion.identity);
        this.GridContainer = matrixGO.transform;
    }

    public void SpawnFoods(List<FoodLevel> foodLevels)
    {
        for (int i = 0; i < foodLevels.Count; i++)
        {
            SpawnFood(foodLevels[i], i+1);
        }
    }

    public void SpawnFood(FoodLevel foodLevel, int index)
    {
        Vector3 pos = GetPosition(foodLevel.PoseList);
        pos.y = 1.41f;
        IngredientView foodViewGO = Instantiate(foodLevel.Prefab, pos, foodLevel.Prefab.transform.rotation, this.transform).GetComponent<IngredientView>();
        string newName = foodViewGO.name.Replace("(Clone)", " " + index.ToString());
        foodViewGO.name = newName;

        if (foodLevel.isCooked)
        {
            foodViewGO.SetInitCook();
        }

        foodViewGO.poses = new List<Vector2Int>(foodLevel.PoseList);

        foreach (var pose in foodLevel.PoseList)
        {
            if (foodViewGO.FoodType == FoodType.Rock)
            {
                this.ingredientGrid[pose.x, pose.y].index = index + 500;
            }
            else
            {
                this.ingredientGrid[pose.x, pose.y].index = index;
            }
            foreach (var direction in foodLevel.DirectionList)
            {
                this.ingredientGrid[pose.x, pose.y].directions.Add(direction);
            }
        }
    }

    private void Update()
    {
        Direction dir = this.GetDirection();

        TouchTime += Time.deltaTime;

        //Debug.Log(dir);

        if (dir != Direction.None)
        {
            this.Dir = dir;
        }

        if (Input.touchCount <= 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);

        Vector3 secondPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0f));

        float x = secondPos.x - firstPos.x;
        float y = secondPos.z - firstPos.z;

        bool isCancel = false;

        if (Mathf.Abs(x) <= 0.05f && Mathf.Abs(y) <= 0.05f)
        {
            isCancel = true;
        }

        if (touch.phase == TouchPhase.Ended /*&& !CustomerManager.Instance.isSwitching*/)
        {
            TimeWait = 0f;

            if (!isCancel)
            {

                if (currentView != null)
                {
                    //currentView.ReturnFirstPosition();

                    if (TutorialManager.Instance != null)
                    {
                        if (!TutorialManager.Instance.isDone)
                        {
                            if (TutorialManager.Instance.CheckStep(currentView?.name, Dir))
                            {
                                currentView.HideHightLight();
                                Move(Dir, currentView.poses);
                            }
                        }
                        else
                        {
                            currentView.HideHightLight();
                            Move(Dir, currentView.poses);
                        }

                    }
                    else
                    {
                        currentView.HideHightLight();
                        Move(Dir, currentView.poses);
                    }
                }
            }
            else
            {
                if (currentView != null)
                {
                    currentView.HideHightLight();
                    currentView.ReturnFirstPosition();
                }
            }
            

            currentView = null;
        }
        else
        {
            if (currentView != null && TouchTime >= 0.5f)
            {
                currentView.Nudge(firstPos, secondPos);
            }
        }
    }

    private Direction GetDirection()
    {
        Direction dir = Direction.None;
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
            if (currentView != null)
            {
                currentView.ReturnFirstPosition();
            }
            Debug.Log("Invalid move");

            if (currentView != null )
            {
                currentView.Shake();
            }
            return;
        }

        isPressing = true;

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

                SolveMatrix(p, poses, dir, id, ds);

                break;
            case Direction.Right:
                p = poses[poses.Count-1] + offset;

                SolveMatrix(p, poses, dir, id, ds);

                break;
            case Direction.Up:
                p = poses[0] + offset;

                SolveMatrix(p, poses, dir, id, ds);

                break;
            case Direction.Down:
                p = poses[poses.Count - 1] + offset;

                SolveMatrix(p, poses, dir, id, ds);

                break;
        }

        if (isSetCookedSuccess)
        {
            return;
        }

        if (isSetCookedFailure)
        {
            poses = currentView.poses;

            for (int i = 0; i < poses.Count; i++)
            {
                ingredientGrid[poses[i].x, poses[i].y].index = id;
                ingredientGrid[poses[i].x, poses[i].y].directions = ds;
            }

            return;
        }


        if (isPort)
        {
            for (int i = 0; i < this.newPosses.Count; i++)
            {
                ingredientGrid[this.newPosses[i].x, this.newPosses[i].y].index = id;
                ingredientGrid[this.newPosses[i].x, this.newPosses[i].y].directions = newDs;
            }

            Move(newDs[0], this.newPosses);

            return;
        }

        currentView.SetPosesAndMove(poses, TimeWait);

        for (int i = 0; i < poses.Count; i++)
        {
            ingredientGrid[poses[i].x, poses[i].y].index = id;
            ingredientGrid[poses[i].x, poses[i].y].directions = ds;
        }


        return;
    }

    private void SolveMatrix(Vector2Int p, List<Vector2Int> poses, Direction dir, int id, List<Direction> ds)
    {
        isSetCookedSuccess = false;
        isSetCookedFailure = false;
        isPort = false;

        if (!IsInMatrix(p))
        {
            if (currentView.isCooked)
            {
                if (QueueController.Instance.IsAvailablePos())
                {
                    //Debug.Log("Cut ra");
                    currentView.MoveOut(GetPosition(poses), QueueController.Instance.GetAvailablePos(), TimeWait);
                    QueueController.Instance.AddQueue(currentView.FoodType);
                    isSetCookedSuccess = true;
                    //Destroy(currentView.gameObject);
                }
                else
                {
                    currentView.Shake(TimeWait);
                    isSetCookedFailure = true;
                }

                //Debug.Log(isSetCookedSuccess + " " + isSetCookedFailure);

                return;
            }
        }
        else if (ingredientGrid[p.x, p.y].index == 5000)
        {
            //Debug.Log(p.x + " " + p.y);

            foreach (DirectionCombo combo in ingredientGrid[p.x, p.y].portView.DirectionCombos)
            {
                if (combo.directionIn == dir)
                {
                    dir = combo.directionOut[0];

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

                    List<Vector2Int> newPosses = new List<Vector2Int>();

                    if (combo.angle != 0)
                    {
                        newPosses = RotateIngredient(p, poses, combo.angle);
                    }
                    else
                    {
                        int mul = 3;
                        if (currentView.FoodType == FoodType.Mushroom)
                        {
                            mul = 2;
                        }

                        for (int i = 0; i < poses.Count; i++)
                        {
                            newPosses.Add(poses[i] + offset * mul);
                        }
                    }

                    //foreach (Vector2Int vector2Int in newPosses)
                    //{
                    //    Debug.Log(vector2Int);
                    //}

                    if (newPosses.Count > 1)
                    {
                        if (!(newPosses[0].x <= newPosses[1].x && newPosses[0].y <= newPosses[1].y))
                        {
                            Vector2Int temp = newPosses[0];
                            newPosses[0] = newPosses[1];
                            newPosses[1] = temp;
                        }
                    }

                    for (int i = 0; i < newPosses.Count; i++)
                    {
                        if (!IsInMatrix(newPosses[i]) || ingredientGrid[newPosses[i].x, newPosses[i].y].index != 0)
                        {
                            return;
                        }
                    }

                    isPort = true;
                    this.newPosses = new List<Vector2Int>(newPosses);
                    this.newDs = new List<Direction>(combo.directionOut);

                    //foreach (Vector2Int vector2Int in newPosses)
                    //{
                    //    Debug.Log(vector2Int);
                    //}

                    currentView.SetPosesAndMove(poses, TimeWait);

                    IngredientView foodPrefab;

                    if (combo.angle != 0)
                    {
                        foodPrefab = currentView.convertObj;
                    }
                    else
                    {
                        foodPrefab = currentView;
                    }

                    currentView.Hide(0.5f + TimeWait);

                    Vector3 pos = GetPosition(newPosses);
                    pos.y = 1.41f;
                    IngredientView foodViewGO = Instantiate(foodPrefab, pos, foodPrefab.transform.rotation, this.transform).GetComponent<IngredientView>();
                    foodViewGO.transform.localScale = Vector3.zero;
                    string newName = foodViewGO.name.Replace("(Clone)", " " + id.ToString());
                    foodViewGO.name = newName;

                    if (currentView.isCooked)
                    {
                        foodViewGO.SetInitCook();
                    }

                    foodViewGO.poses = new List<Vector2Int>(newPosses);

                    foodViewGO.Spawn(0.7f + TimeWait);

                    currentView = foodViewGO;

                    TimeWait += 0.9f;

                    return;
                }
            }
        }
        else if (ingredientGrid[p.x, p.y].index > 0 && ingredientGrid[p.x, p.y].index < 500)
        {
            //Debug.Log("Touch");
            if (!currentView.isCooked && currentView.FoodType != FoodType.Rock) currentView.SetCook(TimeWait);
        }
        else if (ingredientGrid[p.x, p.y].index < 0)
        {
            //Debug.Log("Ice Touch");
            if (currentView.isCooked)
            {
                currentView.SetUnCook(TimeWait);
                this.ingredientGrid[p.x, p.y].index += 1;
                //this.ingredientGrid[p.x, p.y].IceView.SetHealth(-this.ingredientGrid[p.x, p.y].index);
                this.ingredientGrid[p.x, p.y].IceView.DecreaseHealth(-this.ingredientGrid[p.x, p.y].index, TimeWait);
                if (this.ingredientGrid[p.x, p.y].index == 0)
                {
                    this.ingredientGrid[p.x, p.y].IceView.Melt(TimeWait);
                    this.ingredientGrid[p.x, p.y].IceView = null;
                }
            }
        }
    }

    private List<Vector2Int> RotateIngredient(Vector2Int root, List<Vector2Int> poses, float angle)
    {
        List<Vector2Int> l = new List<Vector2Int>();
        foreach (Vector2Int pos in poses)
        {
            if (angle == 90)
            {
                Vector2Int pivot = new Vector2Int(pos.x - root.x, pos.y - root.y);
                Vector2Int rotateVector = new Vector2Int(pivot.y, -pivot.x);
                Vector2Int finalVector = new Vector2Int(rotateVector.x + root.x, rotateVector.y + root.y);
                l.Add(finalVector);
            }
            else
            {
                Vector2Int pivot = new Vector2Int(pos.x - root.x, pos.y - root.y);
                Vector2Int rotateVector = new Vector2Int(-pivot.y, pivot.x);
                Vector2Int finalVector = new Vector2Int(rotateVector.x + root.x, rotateVector.y + root.y);
                l.Add(finalVector);
            }
        }
        
        return l;
    }

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

    public void SetFire(List<Vector2Int> poses)
    {
        for (int i = 0; i < poses.Count; i++)
        {
            GridContainer.GetChild(poses[i].x * MatrixSize.y + poses[i].y).GetComponent<Stove>().SetFire();
        }
    }

    public void UnFire(List<Vector2Int> poses)
    {
        for (int i = 0; i < poses.Count; i++)
        {
            GridContainer.GetChild(poses[i].x * MatrixSize.y + poses[i].y).GetComponent<Stove>().UnFire();
        }
    }   
}
