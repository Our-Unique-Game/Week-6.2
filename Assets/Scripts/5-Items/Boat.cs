using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq; // Added for LINQ operations (Concat, Distinct)

/**
 * This component manages the boat's interactions.
 * Handles player pickup, updates allowed tiles for movement, and ensures compatibility with the game system.
 */
public class Boat : MonoBehaviour {
    [SerializeField] AllowedTiles allowedTiles = null; // Reference to the Boat's AllowedTiles
    private bool isPickedUp = false; // Tracks whether the boat is picked up.

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            OnPlayerInteraction(collision.gameObject);
        }
    }

    public void OnPlayerInteraction(GameObject player) {
        if (!isPickedUp) {
            Debug.Log("Boat picked up by player!");
            isPickedUp = true;
            UpdateAllowedTilesForBoat();
        } else {
            Debug.Log("Boat is already being carried!");
        }
    }

    private void UpdateAllowedTilesForBoat() {
        if (allowedTiles != null) {
            // Define water tiles allowed for the boat
            TileBase[] waterTiles = { /* Add specific water tiles here */ };

            // Get the current list of allowed tiles
            TileBase[] currentTiles = allowedTiles.Get();

            // Combine the existing tiles with the waterTiles, avoiding duplicates
            TileBase[] updatedTiles = currentTiles.Concat(waterTiles)
                                                 .Distinct()
                                                 .ToArray();

            // Update the allowed tiles
            allowedTiles.UpdateAllowedTiles(updatedTiles);

            Debug.Log("Allowed tiles updated for boat movement.");
        } else {
            Debug.LogError("AllowedTiles reference is null!");
        }
    }

    public void OnPlayerDrop(GameObject player) {
        if (isPickedUp) {
            Debug.Log("Boat dropped by player.");
            isPickedUp = false;
            ResetAllowedTiles();
        }
    }

    private void ResetAllowedTiles() {
        if (allowedTiles != null) {
            // Reset allowed tiles to their default values
            TileBase[] defaultTiles = allowedTiles.Get();
            allowedTiles.UpdateAllowedTiles(defaultTiles);
            Debug.Log("Allowed tiles reset after boat drop.");
        } else {
            Debug.LogError("AllowedTiles reference is null!");
        }
    }
}
