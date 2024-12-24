using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component manages multiple lists of allowed tiles.
 * The lists can be updated dynamically based on game interactions.
 */
public class AllowedTiles : MonoBehaviour {
    [SerializeField] private TileBase[] defaultAllowedTiles = null; // Default allowed tiles
    private TileBase[] currentAllowedTiles = null;

    private void Awake() {
        // Initialize the currentAllowedTiles with the default tiles
        currentAllowedTiles = defaultAllowedTiles;
    }

    /**
     * Checks if a tile is in the current allowed tiles list.
     * @param tile The tile to check.
     * @return True if the tile is allowed, otherwise false.
     */
    public bool Contains(TileBase tile) {
        return currentAllowedTiles.Contains(tile);
    }

    /**
     * Retrieves the current list of allowed tiles.
     * @return An array of currently allowed tiles.
     */
    public TileBase[] Get() {
        return currentAllowedTiles;
    }

    /**
     * Updates the current allowed tiles dynamically.
     * @param newTiles An array of TileBase objects to set as the new allowed tiles.
     */
    public void UpdateAllowedTiles(TileBase[] newTiles) {
        if (newTiles == null || newTiles.Length == 0) {
            Debug.LogError("New allowed tiles array is null or empty!");
            return;
        }
        currentAllowedTiles = newTiles;
        Debug.Log("Current allowed tiles updated successfully!");
    }

    /**
     * Adds new allowed tiles to the current list dynamically.
     * @param additionalTiles An array of TileBase objects to add to the current allowed tiles.
     */
    public void AddAllowedTiles(TileBase[] additionalTiles) {
        if (additionalTiles == null || additionalTiles.Length == 0) {
            Debug.LogError("Additional tiles array is null or empty!");
            return;
        }

        // Combine existing tiles with additional tiles, avoiding duplicates
        currentAllowedTiles = currentAllowedTiles.Concat(additionalTiles)
                                                 .Distinct()
                                                 .ToArray();
        Debug.Log("New tiles added to allowed tiles successfully!");
    }

    /**
     * Resets the current allowed tiles to the default list.
     */
    public void ResetToDefaultTiles() {
        currentAllowedTiles = defaultAllowedTiles;
        Debug.Log("Current allowed tiles reset to default!");
    }
}
