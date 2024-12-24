using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAvoidance : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float avoidRadius = 5f; // Radius around the player
    [SerializeField] private Tilemap tilemap; // Tilemap for pathfinding
    [SerializeField] private LayerMask enemyLayer; // Layer to detect enemies

    [Header("Avoidance Settings")]
    [SerializeField] private int maxAttempts = 18; // Maximum attempts to find a valid point
    [SerializeField] private float angleStep = 10f; // Angle step size (degrees)
    [SerializeField] private bool logAvoidance = true; // Toggle logs for debugging

    private void Update() {
        // Detect enemies inside the radius
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, avoidRadius, enemyLayer);

        foreach (Collider2D enemyCollider in enemies) {
            EnemyMover enemy = enemyCollider.GetComponent<EnemyMover>();
            if (enemy != null) {
                Log($"Enemy {enemy.name} detected inside radius!");

                // Find the avoidance point based on the enemy's position
                Vector3 avoidancePoint = FindAvoidancePoint(enemy);
                if (avoidancePoint != Vector3.zero) { // Valid target
                    Log($"Enemy {enemy.name} moving to avoidance point: {avoidancePoint}");
                    enemy.SetTarget(avoidancePoint); // Move enemy to target
                } else {
                    Log($"No valid avoidance point found for enemy {enemy.name}");
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

        // Start angle and direction based on position
        float startAngle = isEnemyOnRight ? 0f : 180f; // 0° for right, 180° for left
        float step = isEnemyOnRight ? angleStep : -angleStep; // Clockwise or counter-clockwise

        Log($"Enemy {enemy.name} is on the {(isEnemyOnRight ? "RIGHT" : "LEFT")} side.");

        // Search for the first valid point
        for (int i = 0; i < maxAttempts; i++) {
            float angle = startAngle + (i * step);
            Vector3 direction = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad), 
                Mathf.Sin(angle * Mathf.Deg2Rad), 
                0
            );

            Vector3 candidatePoint = transform.position + direction * avoidRadius;

            // Check if the tile is valid
            if (enemy.IsTileAllowed(candidatePoint)) {
                Log($"Valid point for {enemy.name}: {candidatePoint}");
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

    // Logs messages only if logging is enabled
    private void Log(string message) {
        if (logAvoidance) {
            Debug.Log(message);
        }
    }
}
