using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingShips : MonoBehaviour {

	public Transform seeker, target;
	Grid grid;
	public LineControl lineControl;
	public GameObject waypointPrefab;
	public AIMgr aIMgr;

	
private List<Transform> waypointObjects = new List<Transform>();

	void Awake() {
		grid = GetComponent<Grid> ();
	}

	void Update()
	{
		if (seeker != null && target != null)
		{
			FindPath(seeker.position, target.position);
			seeker = null;
		}
	}

	public void FindPath(Vector3 startPos, Vector3 targetPos) {
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i ++) {
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode) {
				RetracePath(startNode,targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(node)) {
				if (!neighbour.walkable || closedSet.Contains(neighbour)) {
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		List<Node> waypoints = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Add(startNode); // Include start node
		path.Reverse();

		// Detect waypoints (include start and end)
		List<Node> fullWaypoints = new List<Node> { startNode }; // Start point

		Vector2 oldDirection = Vector2.zero;
		for (int i = 1; i < path.Count; i++) {
			int dx = path[i - 1].gridX - path[i].gridX;
			int dy = path[i - 1].gridY - path[i].gridY;
			Vector2 newDirection = new Vector2(dx, dy);
			if (newDirection != oldDirection) {
				fullWaypoints.Add(path[i - 1]); // Add the turning point
			}
			oldDirection = newDirection;
		}
		fullWaypoints.Add(endNode); // End point

		// Save path and waypoints to Grid (for Gizmos)
		grid.path = path;
		grid.waypoints = fullWaypoints;

		// Destroy old waypoint markers
		foreach (var obj in waypointObjects) {
			if (obj != null) Destroy(obj.gameObject);
		}
		waypointObjects.Clear();

		// Instantiate new waypoint markers (start + waypoints + end)
		foreach (var wp in fullWaypoints)
		{
			GameObject wpObj = Instantiate(waypointPrefab, new Vector3(wp.worldPosition.x, 15, wp.worldPosition.z), Quaternion.identity);
			waypointObjects.Add(wpObj.transform);
			aIMgr.HandleMove(SelectionMgr.inst.selectedEntities, new Vector3(wp.worldPosition.x, 0, wp.worldPosition.z));

		}

		// Draw line using LineControl
		if (lineControl != null) {
			lineControl.waypoints = waypointObjects.ToArray();
			lineControl.drawLine();
		}
	}

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}
