using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarAlgorithm : MonoBehaviour
{
    // ѕоле отвечающее за хранение объекта конца пути
    [SerializeField]
    private GameObject target;
    // ѕоле отвечающее за сло€ преп€тстви€
    [SerializeField]
    private LayerMask solidLayer;
    // ѕоле отвечающее за отображени€ пути перемещени€
    [SerializeField]
    private WayDisplayer WayDisplayer;

    // ѕоле отвечающее за хранени€ проверенных нод
    private List<Node> checkedNodes = new List<Node>();
    // ѕоле отвечающее за ожидающие ноды
    private List<Node> waitingNodes = new List<Node>();

    // ѕоле отвечающее за созданный путь
    public List<Vector2> pathToTarget;

    public List<Vector2> getPathResult()
    {
        pathToTarget = GetPath(target.transform.position);
        WayDisplayer.updateWay(pathToTarget);
        return pathToTarget;
    }

    public List<Vector2> GetPath(in Vector2 target)
    {
        pathToTarget = new List<Vector2>();
        checkedNodes = new List<Node>();
        waitingNodes = new List<Node>();

        Vector2 StartPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 TargetPosition = new Vector2(target.x, target.y);

        if (StartPosition == TargetPosition) return pathToTarget;

        Node startNode = new Node(0, StartPosition, TargetPosition, null);
        checkedNodes.Add(startNode);
        waitingNodes.AddRange(GetNeighbourNodes(startNode));
        while (waitingNodes.Count > 0)
        {
            Node nodeToCheck = waitingNodes.Where(x => x.F == waitingNodes.Min(y => y.F)).FirstOrDefault();


            var walkable = !Physics2D.OverlapCircle(nodeToCheck.Position, 0.1f, solidLayer);
            if (!walkable)
            {
                waitingNodes.Remove(nodeToCheck);
                checkedNodes.Add(nodeToCheck);
            }
            else if (walkable)
            {

                if (nodeToCheck.Position == TargetPosition)
                {
                    return CalculatePathFromNode(nodeToCheck);
                }
                waitingNodes.Remove(nodeToCheck);
                if (!checkedNodes.Where(x => x.Position == nodeToCheck.Position).Any())
                {
                    checkedNodes.Add(nodeToCheck);
                    waitingNodes.AddRange(GetNeighbourNodes(nodeToCheck));
                }
            }
        }

        return pathToTarget;
    }

    public List<Vector2> CalculatePathFromNode(in Node node)
    {
        var path = new List<Vector2>();
        Node currentNode = node;

        while (currentNode.PreviousNode != null)
        {
            path.Add(new Vector2(currentNode.Position.x, currentNode.Position.y));
            currentNode = currentNode.PreviousNode;
        }

        return path;
    }

    private List<Node> GetNeighbourNodes(in Node node)
    {
        var Neighbours = new List<Node>();

        Neighbours.Add(new Node(node.G + 1, new Vector2(node.Position.x - 1, node.Position.y),node.TargetPosition,node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(node.Position.x + 1, node.Position.y),node.TargetPosition, node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(node.Position.x, node.Position.y - 1),node.TargetPosition,node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(node.Position.x, node.Position.y + 1),node.TargetPosition,node));
        return Neighbours;
    }

    private void OnDrawGizmos()
    {
        if (pathToTarget != null)
            foreach (var item in pathToTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector2(item.x, item.y), 0.2f);
            }
    }

}

public class Node
{
    public Vector2 Position;
    public Vector2 TargetPosition;
    public Node PreviousNode;
    public int F; // F=G+H
    public int G; // рассто€ние от старта до ноды
    public int H; // рассто€ние от ноды до цели

    public Node(in int g, in Vector2 nodePosition, in Vector2 targetPosition, in Node previousNode)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        PreviousNode = previousNode;
        G = g;
        H = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.y - Position.y);
        F = G + H;
    }
}