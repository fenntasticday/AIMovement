using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingOffice : MonoBehaviour {

	public Transform seeker, target;
	 public List<Transform> intermediateWaypoints; 
	Grid grid;
	public LineControl lineControl;
	public GameObject waypointPrefab;

	
private List<Transform> waypointObjects = new List<Transform>();

	void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        FindFullPath(seeker.position, target.position);
    }

    /// <summary>
    /// Combines multiple paths between waypoints into a single full path.
    /// </summary>
    public void FindFullPath(Vector3 startPos, Vector3 finalTargetPos)
    {
        // Prepare a list of all points to visit: start -> wp1 -> wp2 -> ... -> target
        List<Vector3> pointsToVisit = new List<Vector3>();
        pointsToVisit.Add(startPos);

        foreach (var wp in intermediateWaypoints)
        {
            if (wp != null) pointsToVisit.Add(wp.position);
        }

        pointsToVisit.Add(finalTargetPos);

        // Combine paths
        List<Node> fullPath = new List<Node>();
        List<Node> fullWaypoints = new List<Node>();

        for (int i = 0; i < pointsToVisit.Count - 1; i++)
        {
            List<Node> segmentPath = FindPathSegment(pointsToVisit[i], pointsToVisit[i + 1]);

            if (segmentPath.Count > 0)
            {
                // Avoid duplicating the connecting node
                if (i > 0 && segmentPath.Count > 1)
                    segmentPath.RemoveAt(0);

                fullPath.AddRange(segmentPath);

                // Add waypoints (turning points) for this segment
                List<Node> segmentWaypoints = GetWaypoints(segmentPath);
                if (i > 0 && segmentWaypoints.Count > 0)
                    segmentWaypoints.RemoveAt(0); // Remove overlap

                fullWaypoints.AddRange(segmentWaypoints);
            }
        }

        // Save path and waypoints to grid
        grid.path = fullPath;
        grid.waypoints = fullWaypoints;

        // Destroy old waypoint markers
        foreach (var obj in waypointObjects)
        {
            if (obj != null) Destroy(obj.gameObject);
        }
        waypointObjects.Clear();

        // Instantiate waypoint markers
        foreach (var wp in fullWaypoints)
        {
            GameObject wpObj = Instantiate(waypointPrefab, new Vector3(wp.worldPosition.x, 15, wp.worldPosition.z), Quaternion.identity);
            waypointObjects.Add(wpObj.transform);
        }

        // Draw line
        if (lineControl != null)
        {
            lineControl.waypoints = waypointObjects.ToArray();
            lineControl.drawLine();
        }
    }

    /// <summary>
    /// Pathfinding between two individual points.
    /// </summary>
    private List<Node> FindPathSegment(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(endPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);
        startNode.parent = null;

        while (openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || (openSet[i].fCost == node.fCost && openSet[i].hCost < node.hCost))
                {
                    node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode)
            {
                return RetraceSegmentPath(startNode, targetNode);
            }

            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return new List<Node>(); // No path found
    }

    private List<Node> RetraceSegmentPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();
        return path;
    }

    private List<Node> GetWaypoints(List<Node> path)
    {
        List<Node> waypoints = new List<Node>();
        if (path == null || path.Count == 0) return waypoints;

        Vector2 oldDirection = Vector2.zero;
        waypoints.Add(path[0]); // Start

        for (int i = 1; i < path.Count; i++)
        {
            int dx = path[i - 1].gridX - path[i].gridX;
            int dy = path[i - 1].gridY - path[i].gridY;
            Vector2 newDirection = new Vector2(dx, dy);
            if (newDirection != oldDirection)
            {
                waypoints.Add(path[i - 1]);
            }
            oldDirection = newDirection;
        }

        waypoints.Add(path[^1]); // End
        return waypoints;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return (dstX > dstY) ? 14 * dstY + 10 * (dstX - dstY) : 14 * dstX + 10 * (dstY - dstX);
	}
}
