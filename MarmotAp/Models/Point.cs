﻿namespace MarmotAp.Models;

public class Point
{
    public string Id { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public Point(string ID, int X, int Y)
    {
        Id = ID;
        x = X;
        y = Y;
    }
}
