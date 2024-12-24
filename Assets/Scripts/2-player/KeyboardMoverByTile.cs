using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move, interact with items (goat, boat, pickaxe),
 * and dynamically update allowed tiles. The pickaxe allows the player to transform tiles.
 */
public class KeyboardMoverByTile : MonoBehaviour {
    [SerializeField] Tilemap tilemap = null; // Reference to the Tilemap
    [SerializeField] AllowedTiles defaultAllowedTiles = null; // Default allowed tiles
    [SerializeField] AllowedTiles goatAllowedTiles = null; // Goat-specific allowed tiles
    [SerializeField] AllowedTiles boatAllowedTiles = null; // Boat-specific allowed tiles
    [SerializeField] AllowedTiles pickaxeAllowedTiles = null; // Pickaxe-specific allowed tiles
    [SerializeField] TileBase transformFromTile = null; // Tile to be replaced
    [SerializeField] TileBase transformToTile = null; // Tile to replace with

    private AllowedTiles currentAllowedTiles; // The current combined allowed tiles
    private GameObject interactableObject = null; // Tracks the item the player is near
    private bool isGoatPickedUp = false; // Tracks whether the goat is picked up
    private bool isBoatPickedUp = false; // Tracks whether the boat is picked up
    private bool isPickaxePickedUp = false; // Tracks whether the pickaxe is picked up

    private void Start() {
        // Initialize currentAllowedTiles with default tiles
        currentAllowedTiles = Instantiate(defaultAllowedTiles);
    }

    private void Update() {
        HandleMovement();
        HandleInteraction();
        HandleTileTransformation();
    }

    /**
     * Handles player movement based on arrow keys.
     */
    private void HandleMovement() {
        Vector3 newPosition = NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);

        if (currentAllowedTiles.Contains(tileOnNewPosition)) {
            transform.position = newPosition;
        } else {
        }
    }

    /**
     * Handles interactions with nearby items (goat, boat, pickaxe).
     */
    private void HandleInteraction() {
        if (interactableObject != null && Input.GetKeyDown(KeyCode.Space)) {
            if (interactableObject.CompareTag("Goat")) {
                ToggleGoatPickup();
            } else if (interactableObject.CompareTag("Boat")) {
                ToggleBoatPickup();
            } else if (interactableObject.CompareTag("Pickaxe")) {
                TogglePickaxePickup();
            }
        }
    }

    /**
     * Allows the player to transform tiles if the pickaxe is picked up.
     */
    private void HandleTileTransformation() {
        if (isPickaxePickedUp && Input.GetKeyDown(KeyCode.T)) {
            Vector3 currentPosition = transform.position;
            TileBase currentTile = TileOnPosition(currentPosition);

            if (currentTile == transformFromTile) {
                Vector3Int cellPosition = tilemap.WorldToCell(currentPosition);
                tilemap.SetTile(cellPosition, transformToTile);
                Debug.Log($"Transformed tile at {cellPosition} from {transformFromTile?.name} to {transformToTile?.name}");
            } else {
                Debug.Log($"Cannot transform tile: Current tile is not {transformFromTile?.name}");
            }
        }
    }

    private void ToggleGoatPickup() {
        if (!isGoatPickedUp) {
            isGoatPickedUp = true;
            AddGoatTiles();
            Debug.Log("Goat picked up. Tiles updated.");
        } else {
            isGoatPickedUp = false;
            ResetToDefaultTiles();
            Debug.Log("Goat dropped. Tiles reset to default.");
        }
    }

    private void ToggleBoatPickup() {
        if (!isBoatPickedUp) {
            isBoatPickedUp = true;
            AddBoatTiles();
            Debug.Log("Boat picked up. Tiles updated.");
        } else {
            isBoatPickedUp = false;
            ResetToDefaultTiles();
            Debug.Log("Boat dropped. Tiles reset to default.");
        }
    }

    private void TogglePickaxePickup() {
        if (!isPickaxePickedUp) {
            isPickaxePickedUp = true;
            AddPickaxeTiles();
            Debug.Log("Pickaxe picked up. Tiles updated.");
        } else {
            isPickaxePickedUp = false;
            ResetToDefaultTiles();
            Debug.Log("Pickaxe dropped. Tiles reset to default.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Goat") || collision.CompareTag("Boat") || collision.CompareTag("Pickaxe")) {
            interactableObject = collision.gameObject;
            Debug.Log($"Player is in range of {interactableObject.tag}.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject == interactableObject) {
            Debug.Log($"Player left the range of {interactableObject.tag}.");
            interactableObject = null;
        }
    }

    private TileBase TileOnPosition(Vector3 worldPosition) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    private void AddGoatTiles() {
        if (goatAllowedTiles != null) {
            currentAllowedTiles.AddAllowedTiles(goatAllowedTiles.Get());
            Debug.Log("Goat tiles added to current allowed tiles.");
        } else {
            Debug.LogError("GoatAllowedTiles is null!");
        }
    }

    private void AddBoatTiles() {
        if (boatAllowedTiles != null) {
            currentAllowedTiles.AddAllowedTiles(boatAllowedTiles.Get());
            Debug.Log("Boat tiles added to current allowed tiles.");
        } else {
            Debug.LogError("BoatAllowedTiles is null!");
        }
    }

    private void AddPickaxeTiles() {
        if (pickaxeAllowedTiles != null) {
            currentAllowedTiles.AddAllowedTiles(pickaxeAllowedTiles.Get());
            Debug.Log("Pickaxe tiles added to current allowed tiles.");
        } else {
            Debug.LogError("PickaxeAllowedTiles is null!");
        }
    }

    private void ResetToDefaultTiles() {
        if (defaultAllowedTiles != null) {
            currentAllowedTiles.UpdateAllowedTiles(defaultAllowedTiles.Get());
            Debug.Log("Allowed tiles reset to default.");
        } else {
            Debug.LogError("DefaultAllowedTiles is null!");
        }
    }

    private Vector3 NewPosition() {
        Vector3 move = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            move = Vector3.up;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            move = Vector3.down;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            move = Vector3.left;
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            move = Vector3.right;
        }

        return transform.position + move;
    }

    /**
    * Add this method to expose the player's position.
    */
    public Vector3 GetPlayerPosition() {
        return transform.position;
    }

}
