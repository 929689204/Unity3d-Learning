using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINext {

    public enum Vertex { P3D3B, P2D2, P3D2, P3D1, P3D2B,
                         P3D1B, P3D0, P1D1, P2D2B, P0D2,
                         P0D3B, P2D1B, P0D1, P1D1B, P0D2B, P0D0}
    public enum Edge { P, D, PP, DD, PD, NONE};
    private Edge[,] graph = new Edge[16, 16];
    private bool[] isVisited = new bool[16];
    private int[] parentIndex = new int[16];    // 记录父节点的index，用于回溯找出下一步

    // 画图
    public AINext()
    {
        for (int i = 0; i < 16; i++)
        {
            parentIndex[i] = -1;
            isVisited[i] = false;
            for (int j = 0; j < 16; j++)
            {
                graph[i, j] = Edge.NONE;
            }
        }

        graph[0, 1] = Edge.PD;
        graph[0, 2] = Edge.D;
        graph[0, 3] = Edge.DD;

        graph[1, 0] = Edge.PD;
        graph[1, 4] = Edge.P;

        graph[2, 0] = Edge.D;

        graph[3, 0] = Edge.DD;
        graph[3, 4] = Edge.D;

        graph[4, 1] = Edge.P;
        graph[4, 3] = Edge.D;
        graph[4, 6] = Edge.DD;

        graph[5, 6] = Edge.D;
        graph[5, 7] = Edge.PP;

        graph[6, 4] = Edge.DD;
        graph[6, 5] = Edge.D;

        graph[7, 8] = Edge.PD;
        graph[7, 5] = Edge.PP;

        graph[8, 7] = Edge.PD;
        graph[8, 9] = Edge.PP;

        graph[9, 8] = Edge.PP;
        graph[9, 10] = Edge.D;

        graph[10, 9] = Edge.D;
        graph[10, 12] = Edge.DD;

        graph[11, 12] = Edge.PP;

        graph[12, 10] = Edge.DD;
        graph[12, 11] = Edge.PP;
        graph[12, 13] = Edge.P;
        graph[12, 14] = Edge.D;

        graph[13, 12] = Edge.P;
        graph[13, 15] = Edge.PD;

        graph[14, 12] = Edge.D;
        graph[14, 15] = Edge.DD;

        graph[15, 14] = Edge.DD;
        graph[15, 13] = Edge.PD;
    }

    // 用BFS算法求出下一步
    public Edge GetNext(Vertex curVertex)
    {
        if (curVertex == Vertex.P0D0)
            return Edge.NONE;  
        for (int i = 0; i < 16; i++)
        {
            parentIndex[i] = -1;
            isVisited[i] = false;
        }
        Queue<Vertex> queue = new Queue<Vertex>();
        isVisited[(int)curVertex] = true;
        queue.Enqueue(curVertex);
        while (queue.Count != 0)
        {
            Vertex temp = queue.Dequeue();     
            if (temp == Vertex.P0D0)
                break;     // 到达终点
            for (int i = 0; i < 16; i++)
            {
                if (!isVisited[i] && graph[(int)temp, i] != Edge.NONE)
                {
                    isVisited[i] = true;
                    parentIndex[i] = (int)temp;     // 记录父节点的index
                    queue.Enqueue((Vertex)i);
                }
            }
        }

        ArrayList list = new ArrayList();
        int index = 15;   
        while (true)
        {      
            if (index == -1)
                break;
            list.Add(index);
            index = parentIndex[index];
        }

        Edge nextEdge = Edge.NONE;
        Vertex nextVertex = (Vertex)list[list.Count - 2];
        nextEdge = graph[(int)curVertex, (int)nextVertex];

        return nextEdge;
    }
}