using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]//Ensure the specified component's attachment to game object when this script is attached
public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Tile> path = new List<Tile>();
    [SerializeField] [Range(0f, 5f)] float speed = 1f;
    Enemy enemy;
    // Start is called before the first frame update
    void OnEnable()
    {
        FindPath();
        PlaceInStart();
        StartCoroutine(FollowPath());

    }
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void FindPath()
    {
        path.Clear();
        GameObject parent = GameObject.FindGameObjectWithTag("Path");

        foreach (Transform child in parent.transform)
        {
            Tile waypoint = child.GetComponent<Tile>();
            if (waypoint != null)
            {
                path.Add(waypoint);
            }
        }
    }

    void PlaceInStart()
    {
        transform.position = path[0].transform.position;
    }
    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
    }
    IEnumerator FollowPath()//IEnumerator yield return => coroutine
    {
        foreach (Tile waypoint in path)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = waypoint.transform.position;
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
