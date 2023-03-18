using System;
using UnityEngine;

/// <summary>
/// A typical cell controls a single point on our game grid
/// </summary>
public class Cell:MonoBehaviour
{
    internal int x, y;

    internal Line line=null;
    internal Cell previous=null, post=null;

    internal byte typeIndex = 0;
    internal Color type = Color.white;

    internal bool isHead = false, isSelected=false;

    [SerializeField] SpriteRenderer head;
    [SerializeField] SpriteRenderer glow;
    [SerializeField] SpriteRenderer prevBar;
    [SerializeField] SpriteRenderer postBar;

    /// <summary>
    /// Sets the x and y
    /// </summary>
    /// <param name="x">horizontal position</param>
    /// <param name="y">vertical position</param>
    internal void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;
        SetImages();
    }

    /// <summary>
    /// Checks this and all parents as being equal to cell
    /// </summary>
    /// <param name="cell">the object to check against</param>
    /// <returns>true if connected</returns>
    internal bool IsConnected(Cell cell)
    {
        if (cell.Equals(this)) return true;

        if (previous != null) return previous.IsConnected(cell);

        return false;
    }

    /// <summary>
    /// Unsets this object - or removes it from a line of cells
    /// </summary>
    internal void UnSet()
    {
        if (post != null) post.UnSet();

        if (line != null) line.activeCell = previous;

        if (!isHead)
        {
            type = Color.white;
            typeIndex = 0;
            line = null;
        }

        isSelected = false;

        if(previous != null)
        {
            previous.SetPost(null);
            previous = null;
        }
        SetImages();
    }

    /// <summary>
    /// Set the values for this cell
    /// </summary>
    /// <param name="typeIndex">this type index</param>
    /// <param name="type">this color</param>
    /// <param name="isHead">is it a head?</param>
    /// <param name="line">the connected line</param>
    internal void Set(byte typeIndex, Color type, bool isHead, Line line)
    {
        this.typeIndex = typeIndex;
        this.type = type;
        this.isHead = isHead;
        this.line = line;

        SetImages();
    }

    /// <summary>
    /// Set the values for this cell
    /// </summary>
    /// <param name="typeIndex">this type index</param>
    /// <param name="type">this color</param>
    /// <param name="line">the connected line</param>
    internal void Set(byte typeIndex, Color type, Line line)
    {
        this.typeIndex = typeIndex;
        this.type = type;
        this.line = line;

        SetImages();
    }

    /// <summary>
    /// Sets this as selected
    /// </summary>
    internal void SetSelected()
    {
        isSelected = true;
        SetImages();
    }

    /// <summary>
    /// Sets the cell that comes after this
    /// </summary>
    /// <param name="post">the new post cell</param>
    internal void SetPost(Cell post)
    {
        if (!Equals(post))
        {
            this.post = post;
            SetImages();
        }
    }

    /// <summary>
    /// Sets the cell that comes before this
    /// </summary>
    /// <param name="previous">The new previous cell</param>
    internal void SetPrevious(Cell previous)
    {
        if (!Equals(previous))
        {
            this.previous = previous;
            isSelected = previous != null;
            SetImages();
        }
    }

    /// <summary>
    /// Display the SpriteRenderers of this cell based on the values present
    /// </summary>
    void SetImages()
    {
        SetColors();

        head.gameObject.SetActive(isHead);

        prevBar.gameObject.SetActive(previous != null);

        postBar.gameObject.SetActive(post != null);

        glow.gameObject.SetActive(isSelected);

        #region Set bar rotations -> 90 deg = right
        if(previous != null)
        {
            prevBar.transform.rotation = Quaternion.identity;

            int direction = GetDirection(x, y, previous.x, previous.y);

            //we have an issue
            if (direction < 0)
            {
                previous.SetPost(null);
                previous = null;
                prevBar.gameObject.SetActive(false);
            }
            else
            {
                prevBar.transform.Rotate(new Vector3(0, 0, direction * 90));
            }
        }
        if (post != null)
        {
            postBar.transform.rotation = Quaternion.identity;

            int direction = GetDirection(x, y, post.x, post.y);

            //we have an issue
            if (direction < 0)
            {
                post.SetPrevious(null);
                post = null;
                postBar.gameObject.SetActive(false);
            }
            else
            {
                postBar.transform.Rotate(new Vector3(0, 0, direction * 90));
            }
        }
        #endregion
    }

    /// <summary>
    /// return the direction that a bar should be facing
    /// </summary>
    /// <param name="x0"></param>
    /// <param name="y0"></param>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <returns></returns>
    int GetDirection(int x0, int y0, int x1, int y1)
    {
        if (x0 == x1 && y0 < y1) return 0;//down
        if (x0 < x1 && y0 == y1) return 1; //right
        if (x0 == x1 && y0 > y1) return 2;//up
        if (x0 > x1 && y0 == y1) return 3; //left

        return -1;
    }

    /// <summary>
    /// changes the colors of all of our images
    /// </summary>
    void SetColors()
    {
        head.color = type;
        prevBar.color = type;
        postBar.color = type;
        glow.color = type;
    }

    /// <summary>
    /// Checks if the x and y of this object match
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        return obj is Cell cell &&
               base.Equals(obj) &&
               x == cell.x &&
               y == cell.y;
    }

    /// <summary>
    /// Returns the hash code
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// ran when our mouse is over this object, and we are holding down the "Fire1" button (LMB)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        GameController.instance.ChangeCurrentCell(this);
    }
}
