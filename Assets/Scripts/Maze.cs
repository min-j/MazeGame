using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

    [System.Serializable]
    public class Cell
    {
        public bool visited;
        public GameObject north, east, west,south;
    }

    public GameObject wall, floor, wallTrap, spikeTrap, openFloor, coinFloor;
    public float wallLength;
    public int size;

    private int upDown = 8;
    private int currentCell = 0;
    private int total;
    private int visited = 0;
    private int currentN = 0;
    private int back = 0;
    private int wallBreak = 0;
    private bool started = false;
    private Vector3 initialPos;
    private GameObject wallHold;
    private Cell[] cells;
    private List<int> lastCell;

	// Use this for initialization
	void Start () {
        CreateWalls();
        InvokeRepeating("showWallTraps", 0.0f, 5.0f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void showWallTraps()
    {
        upDown = -upDown;
        GameObject[] arr = GameObject.FindGameObjectsWithTag("WallTrap");
        foreach (GameObject g in arr)
        {
            g.transform.position += new Vector3(0, upDown, 0);
        }
    }

    void CreateWalls ()
    {
        wallHold = new GameObject();
        wallHold.name = "Maze";
        initialPos = new Vector3((-size / 2) + wallLength / 2, 0.0f, (-size / 2) + wallLength / 2);
        Vector3 myPos = initialPos;
        GameObject tempWall;
        GameObject tempFloor;
        GameObject tempTrap;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j <= size; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength) - wallLength / 2, 0.0f, 
                                    initialPos.z + (i * wallLength) - wallLength / 2);
                tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                tempWall.transform.parent = wallHold.transform;
            }
        }

        for (int i = 0; i <= size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength), 0.0f,
                                    initialPos.z + (i * wallLength) - wallLength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;
                tempWall.transform.parent = wallHold.transform;
            }
        }

        for (int i = 0; i <= size - 1; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int r = Random.Range(0, size);
                GameObject g;
                if ((i == 0 && j == 0) || (i == size && j == size))
                {
                    g = floor;
                }
                else if (r == 2)
                {
                    g = openFloor;
                }
                else if (r == 4 || r == 6)
                {
                    g = wallTrap;
                }
                else if (r == 8)
                {
                    g = spikeTrap;
                }
                else if (r % 5 == 0)
                {
                    g = coinFloor;
                }
                else
                {
                    g = floor;
                }
                myPos = new Vector3(initialPos.x + (j * wallLength), -0.5f, initialPos.z + (i * wallLength) - wallLength / 2);
                if (g == openFloor)
                {
                    tempTrap = Instantiate(g, myPos, Quaternion.identity) as GameObject;
                    tempTrap.transform.parent = wallHold.transform;
                    continue;
                }
                if (g == wallTrap)
                {
                    tempFloor = Instantiate(floor, myPos, Quaternion.identity) as GameObject;
                    tempTrap = Instantiate(g, myPos, Quaternion.identity) as GameObject;
                    tempFloor.transform.parent = wallHold.transform;
                    tempTrap.transform.parent = wallHold.transform;
                }
                tempFloor = Instantiate(g, myPos, Quaternion.identity) as GameObject;
                tempFloor.transform.parent = wallHold.transform;

            }
        }

        CreateCells();
    }

    void CreateCells ()
    {
        lastCell = new List<int>();
        lastCell.Clear();
        total = size * size;
        GameObject[] allWalls;
        int children = wallHold.transform.childCount;
        allWalls = new GameObject[children];
        cells = new Cell[total];
        int eastwest = 0;
        int northsouth = 0;
        int count = 0;

        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallHold.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = new Cell();
            cells[i].east = allWalls[eastwest];
            cells[i].south = allWalls[northsouth + (size + 1) * size];
            if (count == size)
            {
                eastwest += 2;
                count = 0;
            }
            else
            {
                eastwest++;
            }
            count++;
            northsouth++;
            cells[i].west = allWalls[eastwest];
            cells[i].north = allWalls[(northsouth + (size + 1) * size) + size - 1];
        }

        CreateMaze();
    }

    void FindNeighbor()
    {
        int length = 0;
        int[] n = new int[4];
        int[] walls = new int[4];
        int check = 0;
        check = (currentCell + 1) / size;
        check -= 1;
        check *= size;
        check += size;

        //west 
        if (currentCell + 1 < total && (currentCell + 1) != check)
        {
            if (cells[currentCell + 1].visited == false)
            {
                n[length] = currentCell+1;
                walls[length] = 3;
                length++;
            }
        }

        //east 
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (cells[currentCell - 1].visited == false)
            {
                n[length] = currentCell-1;
                walls[length] = 2;
                length++;
            }
        }

        //north 
        if (currentCell + size < total)
        {
            if (cells[currentCell + size].visited == false)
            {
                n[length] = currentCell + size;
                walls[length] = 1;
                length++;
            }
        }

        //south 
        if (currentCell - size >= 0) 
        {
            if (cells[currentCell - size].visited == false)
            {
                n[length] = currentCell - size;
                walls[length] = 4;
                length++;
            }
        }

        if (length != 0)
        {
            int num = Random.Range(0, length);
            currentN = n[num];
            wallBreak = walls[num];
        }
        else
        {
            if (back > 0)
            {
                currentCell = lastCell[back];
                back--;
            }
        }
    }

    void BreakWall()
    {
        switch (wallBreak)
        {
            case 1: 
                Destroy(cells[currentCell].north); break;
            case 2:
                Destroy(cells[currentCell].east); break;
            case 3:
                Destroy(cells[currentCell].west); break;
            case 4:
                Destroy(cells[currentCell].south); break;
        }
    }

    void CreateMaze ()
    {
        while (visited < total)
        {
            if (started)
            {
                FindNeighbor();
                if (cells[currentN].visited == false && cells[currentCell].visited == true)
                {
                    BreakWall();
                    cells[currentN].visited = true;
                    visited++;
                    lastCell.Add(currentCell);
                    currentCell = currentN;
                    if (lastCell.Count > 0)
                    {
                        back = lastCell.Count-1;
                    }
                }

            }
            else
            {
                currentCell = Random.Range(0, total);
                cells[currentCell].visited = true;
                visited++;
                started = true;
            }
        }
    }

}
