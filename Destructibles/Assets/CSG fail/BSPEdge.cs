using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BSPEdge
{
    Vertex start;
    Vertex end;

    public BSPEdge(Vertex _start, Vertex _end)
    {
        this.start = _start;
        this.end = _end;
        this.start.beginning = true;
        this.end.beginning = false;
    }
}
