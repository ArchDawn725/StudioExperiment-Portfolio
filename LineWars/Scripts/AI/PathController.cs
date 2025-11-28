using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PathController : MonoBehaviour
{
    [SerializeField] private int max;
    [SerializeField] private Transform[] startingPositions;
    [SerializeField] private Transform[] endPositions;

    public List<Vector3> path_1_0 = new List<Vector3>();
    public List<Vector3> path_1_1 = new List<Vector3>();
    public List<Vector3> path_1_2 = new List<Vector3>();
    public List<Vector3> path_1_3 = new List<Vector3>();

    public List<Vector3> path_2_0 = new List<Vector3>();
    public List<Vector3> path_2_1 = new List<Vector3>();
    public List<Vector3> path_2_2 = new List<Vector3>();
    public List<Vector3> path_2_3 = new List<Vector3>();

    public List<Vector3> path_3_0 = new List<Vector3>();
    public List<Vector3> path_3_1 = new List<Vector3>();
    public List<Vector3> path_3_2 = new List<Vector3>();
    public List<Vector3> path_3_3 = new List<Vector3>();
    public static PathController Instance { get; private set; }
    private void OnEnable() { Instance = this; }

    public void StartUp()
    {
        path_1_0.Clear();
        path_1_1.Clear();
        path_1_2.Clear();
        path_1_3.Clear();

        path_2_0.Clear();
        path_2_1.Clear();
        path_2_2.Clear();
        path_2_3.Clear();

        path_3_0.Clear();
        path_3_1.Clear();
        path_3_2.Clear();
        path_3_3.Clear();

        StartCoroutine(GeneratePath_1_0());
    }

    [SerializeField] private Tilemap tilemap1;
    [SerializeField] private Tilemap tilemap2;
    [SerializeField] private Tilemap tilemap3;
    private bool isPositionPainted(Vector3 pos, int type)
    {
        Vector3Int cellPosition = new Vector3Int(0,0,0);
        TileBase tile = null;
        switch (type)
        {
            case 1:
                // Convert the world position to cell position
                cellPosition = tilemap1.WorldToCell(pos);

                // Get the tile at the given cell position
                tile = tilemap1.GetTile(cellPosition);
                break;
            case 2:
                // Convert the world position to cell position
                cellPosition = tilemap2.WorldToCell(pos);

                // Get the tile at the given cell position
                tile = tilemap2.GetTile(cellPosition);
                break;
            case 3:
                // Convert the world position to cell position
                cellPosition = tilemap3.WorldToCell(pos);

                // Get the tile at the given cell position
                tile = tilemap3.GetTile(cellPosition);
                break;
        }


        // Check if the tile is not null
        return tile != null;
    }
    private bool isPositionClaimed(Vector3 pos, int type)
    {
        switch(type)
        {
            default: return false;

            case 0: if (path_1_0.Contains(pos)) { return true; } return false;
            case 1: if (path_1_0.Contains(pos)) { return true; } if (path_1_1.Contains(pos)) { return true; } break;
            case 2: if (path_1_0.Contains(pos)) { return true; } if (path_1_1.Contains(pos)) { return true; } if (path_1_2.Contains(pos)) { return true; } break;
            case 3: if (path_1_0.Contains(pos)) { return true; } if (path_1_1.Contains(pos)) { return true; } if (path_1_2.Contains(pos)) { return true; } if (path_1_3.Contains(pos)) { return true; } break;

            case 4: if (path_2_0.Contains(pos)) { return true; } return false;
            case 5: if (path_2_0.Contains(pos)) { return true; } if (path_2_1.Contains(pos)) { return true; } break;
            case 6: if (path_2_0.Contains(pos)) { return true; } if (path_2_1.Contains(pos)) { return true; } if (path_2_2.Contains(pos)) { return true; } break;
            case 7: if (path_2_0.Contains(pos)) { return true; } if (path_2_1.Contains(pos)) { return true; } if (path_2_2.Contains(pos)) { return true; } if (path_2_3.Contains(pos)) { return true; } break;

            case 8: if (path_3_0.Contains(pos)) { return true; } return false;
            case 9: if (path_3_0.Contains(pos)) { return true; } if (path_3_1.Contains(pos)) { return true; } break;
            case 10: if (path_3_0.Contains(pos)) { return true; } if (path_3_1.Contains(pos)) { return true; } if (path_3_2.Contains(pos)) { return true; } break;
            case 11: if (path_3_0.Contains(pos)) { return true; } if (path_3_1.Contains(pos)) { return true; } if (path_3_2.Contains(pos)) { return true; } if (path_3_3.Contains(pos)) { return true; } break;
        }
        return false;
    }
    private IEnumerator GeneratePath_1_0()
    {
        path_1_0.Add(startingPositions[0].position);
        bool finished = false;
        while (!finished)
        {
            if (path_1_0.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_1_0[path_1_0.Count - 1] == endPositions[0].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_1_0.Count > 1) { if (path_1_0[path_1_0.Count - 1] == path_1_0[path_1_0.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }
            
            Vector3 newPos = path_1_0[path_1_0.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 0)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 0)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 0)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 0)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }
            
            path_1_0.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GeneratePath_1_1());
    }

    private IEnumerator GeneratePath_1_1()
    {
        path_1_1.Add(startingPositions[1].position);
        bool finished = false;
        while (!finished)
        {
            if (path_1_1.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_1_1[path_1_1.Count - 1] == endPositions[1].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_1_1.Count > 1) { if (path_1_1[path_1_1.Count - 1] == path_1_1[path_1_1.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }

            Vector3 newPos = path_1_1[path_1_1.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 1)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 1)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 1)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 1)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }

            path_1_1.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GeneratePath_1_2());
    }

    private IEnumerator GeneratePath_1_2()
    {
        path_1_2.Add(startingPositions[2].position);
        bool finished = false;
        while (!finished)
        {
            if (path_1_2.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_1_2[path_1_2.Count - 1] == endPositions[2].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_1_2.Count > 1) { if (path_1_2[path_1_2.Count - 1] == path_1_2[path_1_2.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }

            Vector3 newPos = path_1_2[path_1_2.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 2)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 2)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 2)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 2)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }

            path_1_2.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GeneratePath_1_3());
    }

    private IEnumerator GeneratePath_1_3()
    {
        path_1_3.Add(startingPositions[3].position);
        bool finished = false;
        while (!finished)
        {
            if (path_1_3.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_1_3[path_1_3.Count - 1] == endPositions[3].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_1_3.Count > 1) { if (path_1_3[path_1_3.Count - 1] == path_1_3[path_1_3.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }

            Vector3 newPos = path_1_3[path_1_3.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 3)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 3)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 3)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 1) && !isPositionClaimed(newPosCheck, 3)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }

            path_1_3.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GeneratePath_2_0());
    }
    private IEnumerator GeneratePath_2_0()
    {
        path_2_0.Add(startingPositions[4].position);
        bool finished = false;
        while (!finished)
        {
            if (path_2_0.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_2_0[path_2_0.Count - 1] == endPositions[4].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_2_0.Count > 1) { if (path_2_0[path_2_0.Count - 1] == path_2_0[path_2_0.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }

            Vector3 newPos = path_2_0[path_2_0.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 4)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 4)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 4)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 4)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }

            path_2_0.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GeneratePath_2_1());
    }
    private IEnumerator GeneratePath_2_1()
    {
        path_2_1.Add(startingPositions[5].position);
        bool finished = false;
        while (!finished)
        {
            if (path_2_1.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_2_1[path_2_1.Count - 1] == endPositions[5].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_2_1.Count > 1) { if (path_2_1[path_2_1.Count - 1] == path_2_1[path_2_1.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }

            Vector3 newPos = path_2_1[path_2_1.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 5)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 5)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 5)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 5)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }

            path_2_1.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GeneratePath_2_2());
    }
    private IEnumerator GeneratePath_2_2()
    {
        path_2_2.Add(startingPositions[6].position);
        bool finished = false;
        while (!finished)
        {
            if (path_2_2.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_2_2[path_2_2.Count - 1] == endPositions[6].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_2_2.Count > 1) { if (path_2_2[path_2_2.Count - 1] == path_2_2[path_2_2.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }

            Vector3 newPos = path_2_2[path_2_2.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 6)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 6)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 6)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 6)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }

            path_2_2.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GeneratePath_2_3());
    }
    private IEnumerator GeneratePath_2_3()
    {
        path_2_3.Add(startingPositions[7].position);
        bool finished = false;
        while (!finished)
        {
            if (path_2_3.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_2_3[path_2_3.Count - 1] == endPositions[7].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_2_3.Count > 1) { if (path_2_3[path_2_3.Count - 1] == path_2_3[path_2_3.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }

            Vector3 newPos = path_2_3[path_2_3.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 7)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 7)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 7)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 2) && !isPositionClaimed(newPosCheck, 7)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }

            path_2_3.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GeneratePath_3_0());
    }
    private IEnumerator GeneratePath_3_0()
    {
        path_3_0.Add(startingPositions[8].position);
        bool finished = false;
        while (!finished)
        {
            if (path_3_0.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_3_0[path_3_0.Count - 1] == endPositions[8].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_3_0.Count > 1) { if (path_3_0[path_3_0.Count - 1] == path_3_0[path_3_0.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }

            Vector3 newPos = path_3_0[path_3_0.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 8)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 8)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 8)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 8)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }

            path_3_0.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GeneratePath_3_1());
    }
    private IEnumerator GeneratePath_3_1()
    {
        path_3_1.Add(startingPositions[9].position);
        bool finished = false;
        while (!finished)
        {
            if (path_3_1.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_3_1[path_3_1.Count - 1] == endPositions[9].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_3_1.Count > 1) { if (path_3_1[path_3_1.Count - 1] == path_3_1[path_3_1.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }

            Vector3 newPos = path_3_1[path_3_1.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 9)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 9)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 9)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 9)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }

            path_3_1.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GeneratePath_3_2());
    }
    private IEnumerator GeneratePath_3_2()
    {
        path_3_2.Add(startingPositions[10].position);
        bool finished = false;
        while (!finished)
        {
            if (path_3_2.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_3_2[path_3_2.Count - 1] == endPositions[10].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_3_2.Count > 1) { if (path_3_2[path_3_2.Count - 1] == path_3_2[path_3_2.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }

            Vector3 newPos = path_3_2[path_3_2.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 10)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 10)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 10)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 10)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }

            path_3_2.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GeneratePath_3_3());
    }
    private IEnumerator GeneratePath_3_3()
    {
        path_3_3.Add(startingPositions[11].position);
        bool finished = false;
        while (!finished)
        {
            if (path_3_3.Count > max) { Debug.LogError("Path ran away"); finished = true; break; }
            if (path_3_3[path_3_3.Count - 1] == endPositions[11].position) { Debug.Log("Finished"); finished = true; break; }
            if (path_3_3.Count > 1) { if (path_3_3[path_3_3.Count - 1] == path_3_3[path_3_3.Count - 2]) { Debug.LogError("Cannot create path! Path incomplete!"); finished = true; break; } }

            Vector3 newPos = path_3_3[path_3_3.Count - 1];

            Vector3 newPosCheck = newPos;
            newPosCheck.x += 0.5f;
            if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 11)) { newPos = newPosCheck; }
            else
            {
                newPosCheck = newPos;
                newPosCheck.y += 0.5f;
                if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 11)) { newPos = newPosCheck; }
                else
                {
                    newPosCheck = newPos;
                    newPosCheck.x -= 0.5f;
                    if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 11)) { newPos = newPosCheck; }
                    else
                    {
                        newPosCheck = newPos;
                        newPosCheck.y -= 0.5f;
                        if (isPositionPainted(newPosCheck, 3) && !isPositionClaimed(newPosCheck, 11)) { newPos = newPosCheck; }
                        else { Debug.Log("Not found"); }
                    }
                }
            }

            path_3_3.Add(newPos);
            yield return new WaitForSeconds(0.01f);
        }
        //StartCoroutine(GeneratePath_1_1());
    }
}
