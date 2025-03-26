using Godot;
using System.Collections.Generic;

public partial class ConnectionLayer : Control
{
    public List<Vector2> SlotPositions = new List<Vector2>();

    public override void _Draw()
    {
        if (SlotPositions.Count < 2) return;

        for (int i = 0; i < SlotPositions.Count - 1; i++)
        {
            Vector2 start = SlotPositions[i];
            Vector2 end = SlotPositions[i + 1];

            DrawLine(start, end, Colors.Yellow, 2);
        }
    }

    public void UpdateConnections(List<Vector2> newPositions)
    {
        SlotPositions = newPositions;
        QueueRedraw();
    }
}