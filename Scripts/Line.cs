using UnityEngine;

public class Line
{
    internal Cell headA, headB, activeCell;

    internal byte typeIndex;
    internal Color type;

    /// <summary>
    /// Initializes the values in this line
    /// </summary>
    /// <param name="headA"></param>
    /// <param name="headB"></param>
    /// <param name="type"></param>
    /// <param name="typeIndex"></param>
    internal void Initialize(Cell headA, Cell headB, Color type, byte typeIndex)
    {
        this.headA = headA;
        this.headB = headB;
        this.type = type;
        this.typeIndex = typeIndex;

        headA.Set(typeIndex, type, true, this);
        headB.Set(typeIndex, type, true, this);
    }

    /// <summary>
    /// Adds a cell to this line
    /// </summary>
    /// <param name="cell"></param>
    internal void Add(Cell cell)
    {
        //This cell is part of another line, destroy that line first.
        if(cell.typeIndex !=0 && cell.typeIndex != typeIndex)
        {
            cell.UnSet();
            Add(cell);
        }
        //set the head of the line
        else if (activeCell == null)
        {
            if (cell.isHead)
            {
                cell.SetSelected();
                activeCell = cell;
            }
        }
        //add a new cell to this line
        else
        {
            //we've looped around to the head of the line
            if(activeCell.IsConnected(cell) && cell.isHead)
            {
                if (cell.post != null) cell.post.UnSet();

                cell.SetSelected();
                activeCell = cell;
            }

            //we have this cell in the line and we want to undo the line until we get back to it 
            if(cell.typeIndex == typeIndex && !cell.isHead)
            {
                if (cell.post != null) cell.post.UnSet();
            }
            //this cell is not a part of this line or other lines, add it
            else
            {
                cell.Set(typeIndex, type, this);
                activeCell.SetPost(cell);
                cell.SetPrevious(activeCell);
                activeCell = cell;

                if (IsConnected()) InputHandler.instance.CancelClick();
            }
        }
    }

    /// <summary>
    /// Determines if headA is connected to headB
    /// </summary>
    /// <returns>true if connected</returns>
    internal bool IsConnected()
    {
        return headA.IsConnected(headB) || headB.IsConnected(headA);
    }

    /// <summary>
    /// Empties this line
    /// </summary>
    internal void Clear()
    {
        if (headA.post != null) headA.UnSet();
        if (headB.post != null) headB.UnSet();
    }
}
