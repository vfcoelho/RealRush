using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]//Ensure the specified component's attachment to game object when this script is attached
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] float speed = 1f;
    List<Node> path = new List<Node>();
    Enemy enemy;
    GridManager gridManager;
    Pathfinder pathfinder;

    void OnEnable()
    {
        PlaceInStart();
        RecalculatePath(true);
    }
    void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();
        if (resetPath)
        {
            coordinates = pathfinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }
        StopAllCoroutines();
        path.Clear();
        path = pathfinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    void PlaceInStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }
    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
    }
    IEnumerator FollowPath()//IEnumerator yield return => coroutine
    {
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;//Gets the time needed for each frame to execute
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);//Linear Interpolation - soothes movement between 2 points
                yield return new WaitForEndOfFrame();//deffer control (yield) and return it after the end of frame execution (WaitForSeconds)
            }
            // transform.position = waypoint.transform.position;
            // yield return new WaitForSeconds(waitTime);//deffer control (yield) and return it after one second (WaitForSeconds)
        }
        // Destroy(gameObject);
        FinishPath();
    }

}
