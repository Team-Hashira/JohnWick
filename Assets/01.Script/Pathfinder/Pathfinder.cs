using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Hashira.Pathfind
{
    public class Pathfinder : MonoBehaviour
    {
        private class NodeRecord
        {
            public Node node;
            public NodeRecord previousNode;
            public float costSoFar;
            public float estimatedTotalCost;
            public float distanceToTarget;
        }

        public List<Node> FindPath(Node startNode, Node targetNode)
        {
            var open = new List<NodeRecord>();
            var closed = new List<NodeRecord>();

            NodeRecord startRecord = new NodeRecord
            {
                node = startNode,
                costSoFar = 0,
                estimatedTotalCost = GetHeuristic(startNode, targetNode),
                distanceToTarget = GetDistance(startNode, targetNode)
            };

            open.Add(startRecord);
            NodeRecord bestNode = startRecord; //��ǥ�� ���� ������ ������ ��带 ����

            while (open.Count > 0)
            {
                NodeRecord current = GetLowestCostNode(open);

                if (current.node == targetNode)
                {
                    return BuildPath(current);
                }

                //���� ��尡 ��ǥ�� �� ������ �ּ��� ���� ������Ʈ.
                if (current.distanceToTarget < bestNode.distanceToTarget)
                {
                    bestNode = current;
                }

                open.Remove(current);
                closed.Add(current);

                foreach (Node neighbor in current.node.neighbors)
                {
                    if (closed.Exists(x => x.node == neighbor))
                        continue;

                    if (!IsValidMove(current.node, neighbor))
                        continue;

                    float endNodeCost = current.costSoFar + GetDistance(current.node, neighbor);
                    float distanceToTarget = GetDistance(neighbor, targetNode);

                    NodeRecord neighborRecord = open.Find(x => x.node == neighbor);

                    if (neighborRecord != null)
                    {
                        if (neighborRecord.costSoFar <= endNodeCost)
                            continue;

                        open.Remove(neighborRecord);
                    }
                    else
                    {
                        neighborRecord = new NodeRecord();
                        neighborRecord.node = neighbor;
                    }

                    neighborRecord.costSoFar = endNodeCost;
                    neighborRecord.previousNode = current;
                    neighborRecord.distanceToTarget = distanceToTarget;
                    neighborRecord.estimatedTotalCost = endNodeCost + GetHeuristic(neighbor, targetNode);

                    open.Add(neighborRecord);
                }
            }

            //��ǥ���� ������ �� ���� ��� ���� ������ �� �� �ִ� ��� ��ȯ
            return BuildPath(bestNode);
        }

        private float EvaluatePathQuality(List<Node> path, Node targetNode)
        {
            if (path == null || path.Count == 0)
                return float.MaxValue;

            // ������ ���� ��ǥ ���� ������ �Ÿ��� ��ȯ
            return GetDistance(path[path.Count - 1], targetNode);
        }

        //Ư���� ��츦 ����� ���������� �ϴ� ��� ���� �������� ��� �ɷ����� ������ �����.
        private bool IsValidMove(Node currentNode, Node nextNode)
        {
            return true;
        }

        private float GetDistance(Node from, Node to)
        {
            return Vector2.Distance(from.position, to.position);
        }

        private float GetHeuristic(Node from, Node to)
        {
            return Vector2.Distance(from.position, to.position);
        }

        private NodeRecord GetLowestCostNode(List<NodeRecord> openList)
        {
            NodeRecord lowest = openList[0];
            foreach (var node in openList)
            {
                if (node.estimatedTotalCost < lowest.estimatedTotalCost)
                    lowest = node;
            }
            return lowest;
        }

        private List<Node> BuildPath(NodeRecord endNode)
        {
            List<Node> path = new List<Node>();
            NodeRecord current = endNode;

            while (current != null)
            {
                path.Add(current.node);
                current = current.previousNode;
            }

            path.Reverse();
            return path;
        }
    }
}