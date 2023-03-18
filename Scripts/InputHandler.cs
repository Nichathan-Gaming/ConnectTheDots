using UnityEngine;

/// <summary>
/// A typical InputHandler captures the input from the player and allows them to control the game
/// </summary>
public class InputHandler : MonoBehaviour
{
    internal static InputHandler instance;

    [SerializeField] Camera mainCamera;
    [SerializeField] Transform mousePositionTrackingCube;
    [SerializeField] BoxCollider mouseCubeBoxCollider;

    bool isClicking = false;
    bool hasReleased = true;

    [SerializeField] float heightOffset = -1.59f;
    [SerializeField] float widthOffset = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (GameController.isGameActive)
        {
            #region move our mouse tracking cube
            Vector3 v3 = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            mousePositionTrackingCube.position = new Vector3((v3.x + widthOffset) * GridGenerator.horizMouseMoveMultiplier, (v3.y + heightOffset) * GridGenerator.vertMouseMoveMultiplier, 0);
            #endregion

            //get clicks
            if(Input.GetAxisRaw("Fire1") > 0)
            {
                if (!isClicking && hasReleased)
                {
                    isClicking = true;
                    mouseCubeBoxCollider.enabled = true;
                    hasReleased = false;
                }
            }
            else
            {
                CancelClick();
                hasReleased = true;
            }
        }
    }

    /// <summary>
    /// Cancels the players click so the mouse position no longer affects the grid
    /// </summary>
    internal void CancelClick()
    {
        GameController.instance.CheckGame();
        isClicking = false;
        mouseCubeBoxCollider.enabled = false;
    }
}