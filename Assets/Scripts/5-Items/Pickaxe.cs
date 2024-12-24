using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Handles the pickaxe logic, allowing the player to pick it up
 * and use it near specific tiles to transform them.
 */
public class Pickaxe : MonoBehaviour {
    [SerializeField] AllowedTiles allowedTiles = null; // Reference to allowed tiles
    [SerializeField] TileBase targetTile = null; // Tile that can be transformed
    [SerializeField] TileBase transformedTile = null; // Tile to transform into
    [SerializeField] Tilemap tilemap = null; // Reference to the tilemap

    private bool isPickedUp = false; // Tracks if the pickaxe is picked up

    /**
     * Handles player interaction with the pickaxe.
     * @param player The GameObject interacting with the pickaxe.
     */
    public void OnPlayerInteraction(GameObject player) {
        if (!isPickedUp) {
            Debug.Log("Pickaxe picked up by player!");
            isPickedUp = true;
            Destroy(gameObject); // Remove pickaxe object from the scene
        }
    }

    /**
     * Checks if the player is near a target tile and transforms it.
     * @param playerPosition The player's position in the world.
     */
    public void TryTransformTile(Vector3 playerPosition) {
        if (isPickedUp) {
            Vector3Int cellPosition = tilemap.WorldToCell(playerPosition);
            TileBase currentTile = tilemap.GetTile(cellPosition);

            if (currentTile == targetTile) {
                tilemap.SetTile(cellPosition, transformedTile);
                Debug.Log("Tile transformed!");
            }
        }
    }
}
