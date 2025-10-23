using UnityEngine;
using UnityEngine.UI;

public enum CursorState
{
    Default,
    Nav,
    Tunnel,
    Attack
}

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D navCursor;
    public Texture2D tunnelCursor;
    public Texture2D attackCursor;

    public LayerMask navMeshLayer;
    public LayerMask tunnelLayer;
    public LayerMask enemyLayer;
    private CursorState currentState = CursorState.Default;

    [SerializeField] private Player player;

    private void Update()
    {
        UpdateCursorState();
        ApplyCursor();
    }

    private void UpdateCursorState()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, enemyLayer))
        {
            currentState = CursorState.Attack;

            if (Input.GetMouseButtonDown(0))
            {
                player.PlayerAttack(hit.collider.gameObject);
            }
            return;
        }

        if (Physics.Raycast(ray, out hit, 100f, tunnelLayer))
        {
            currentState = CursorState.Tunnel;

            if (Input.GetMouseButtonDown(0))
            {
                player.MoveThroughTunnel();
            }

            return;
        }

        if (Physics.Raycast(ray, out hit, 100f, navMeshLayer))
        {
            currentState = CursorState.Nav;
            return;
        }  

        currentState = CursorState.Default;
    }

    private void ApplyCursor()
    {
        switch (currentState)
        {
            case CursorState.Default:
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                break;

            case CursorState.Nav:
                Cursor.SetCursor(navCursor, Vector2.zero, CursorMode.Auto);
                break;

            case CursorState.Tunnel:
                Cursor.SetCursor(tunnelCursor, Vector2.zero, CursorMode.Auto);
                break;
            case CursorState.Attack:
                Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}