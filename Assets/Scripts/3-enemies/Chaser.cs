using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

/**
 * This component chases or avoids a given target object based on distance.
 */
public class Chaser : TargetMover {
    [Tooltip("The object that we try to chase or avoid")]
    [SerializeField] Transform targetObject = null;

    [Tooltip("Radius in which the enemy avoids the player")]
    [SerializeField] float avoidRadius = 5f;

    [Tooltip("Tilemap for pathfinding")]
    [SerializeField] Tilemap tilemap = null;

    private bool isAvoiding = false; // Tracks avoidance mode

    private void Update() {
        float distanceToTarget = Vector3.Distance(transform.position, targetObject.position);

        if (distanceToTarget < avoidRadius) {
            if (!isAvoiding) {
                isAvoiding = true; // Enter avoidance mode
            }
            MoveToAvoidancePoint(); // Avoid the player
        } else {
            if (isAvoiding) {
                isAvoiding = false; // Exit avoidance mode
            }
            SetTarget(targetObject.position); // Chase the player
        }
    }

    /**
     * Moves the enemy to the farthest valid point on the avoidance circle.
     */
    private void MoveToAvoidancePoint() {
        Vector3 avoidancePoint = FindFarthestPointOnCircle();
        if (avoidancePoint != Vector3.zero) {
            SetTarget(avoidancePoint); // Move to the avoidance point
        }
    }

    /**
     * Finds the farthest valid point on the circle from the player.
     */
    private Vector3 FindFarthestPointOnCircle() {
        List<Vector3> validPoints = new List<Vector3>();

        // Check points around the circle
        int points = 16; // Sample points around the circle
        for (int i = 0; i < points; i++) {
            float angle = (360f / points) * i;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            Vector3 point = targetObject.position + direction * avoidRadius;

            // Ensure point is inside tilemap bounds and walkable
            Vector3Int cellPosition = tilemap.WorldToCell(point);
            if (tilemap.HasTile(cellPosition)) {
                validPoints.Add(point);
            }
        }

        // Find the farthest valid point
        Vector3 farthestPoint = Vector3.zero;
        float maxDistance = 0f;

        foreach (Vector3 point in validPoints) {
            float distance = Vector3.Distance(transform.position, point);
            if (distance > maxDistance) {
                maxDistance = distance;
                farthestPoint = point;
            }
        }

        return farthestPoint;
    }

    public Vector3 TargetObjectPosition() {
        return targetObject.position;
    }

}
