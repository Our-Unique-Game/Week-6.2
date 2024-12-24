using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAvoidance : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float avoidRadius = 5f; // Radius around the player
    [SerializeField] private Tilemap tilemap; // Tilemap for pathfinding
    [SerializeField] private LayerMask enemyLayer; // Layer to detect enemies

    private void Update() {
        // Detect enemies inside the radius
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, avoidRadius, enemyLayer);

        foreach (Collider2D enemyCollider in enemies) {
            EnemyMover enemy = enemyCollider.GetComponent<EnemyMover>();
            if (enemy != null) {
                Debug.Log($"Enemy {enemy.name} detected inside radius!");

                // Find the avoidance point based on the enemy's position
                Vector3 avoidancePoint = FindAvoidancePoint(enemy);
                if (avoidancePoint != Vector3.zero) { // Valid target
                    Debug.Log($"Enemy {enemy.name} moving to avoidance point: {avoidancePoint}");
                    enemy.SetTarget(avoidancePoint); // Move enemy to target
                } else {
                    Debug.Log($"No valid avoidance point found for enemy {enemy.name}");
                }
            }
        }
    }

    /**
     * Finds the avoidance point based on whether the enemy is on the left or right.
     */
    private Vector3 FindAvoidancePoint(EnemyMover enemy) {
        // Determine if the enemy is to the left or right of the player
        bool isEnemyOnRight = enemy.transform.position.x > transform.position.x;

        // Start at 0° (right) or 180° (left) based on position
        float startAngle = isEnemyOnRight ? 0f : 180f;
        float angleStep = isEnemyOnRight ? 10f : -10f; // Clockwise or counter-clockwise
        int attempts = 18; // Half-circle, 180° / 10° = 18 attempts

        Debug.Log($"Enemy {enemy.name} is on the {(isEnemyOnRight ? "RIGHT" : "LEFT")} side.");

        // Search for the first valid point
        for (int i = 0; i < attempts; i++) {
            float angle = startAngle + (i * angleStep);
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            Vector3 candidatePoint = transform.position + direction * avoidRadius;

            // Check if the tile is valid
            if (enemy.IsTileAllowed(candidatePoint)) {
                Debug.Log($"Valid point for {enemy.name}: {candidatePoint}");
                return candidatePoint; // Return the first valid point found
            }
        }

        // No valid point found
        return Vector3.zero;
    }

    // Debugging: Draw the radius in the Editor
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidRadius);
    }
}
