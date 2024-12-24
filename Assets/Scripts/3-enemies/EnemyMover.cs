using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMover : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private Tilemap tilemap; // Tilemap for pathfinding
    [SerializeField] private AllowedTiles allowedTiles; // Allowed tiles
    [SerializeField] private float speed = 3f; // Movement speed

    private Vector3 targetPosition; // The target position to move toward
    private bool isMoving = false; // Controls whether the enemy is allowed to move

    private void Update() {
        if (isMoving) {
            MoveDirectly(); // Move directly to the target
        }
    }

    // Sets the target position for movement
    public void SetTarget(Vector3 newTarget) {
        if (IsTileAllowed(newTarget)) { // Validate allowed tile
            targetPosition = newTarget;
            isMoving = true; // Enable movement when valid target is set
            Debug.Log($"Enemy {name} target set to: {newTarget}");
        } else {
            isMoving = false; // Disable movement if target is invalid
            Debug.Log($"Enemy {name} cannot move to invalid tile: {newTarget}");
        }
    }

    // Moves directly toward the target position
    private void MoveDirectly() {
        // Move step-by-step toward the target
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Stop moving when close enough to target
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f) {
            isMoving = false;
            Debug.Log($"Enemy {name} reached target: {targetPosition}");
        }
    }

    // Checks if a tile is valid based on Allowed Tiles
    public bool IsTileAllowed(Vector3 worldPosition) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        TileBase tile = tilemap.GetTile(cellPosition);
        bool isValid = allowedTiles.Contains(tile); // Validate against Allowed Tiles
        Debug.Log($"Checking tile at {worldPosition} - Valid: {isValid}");
        return isValid;
    }

    // Stop movement if called explicitly (e.g., outside radius)
    public void StopMoving() {
        isMoving = false;
        Debug.Log($"Enemy {name} stopped moving");
    }
}
