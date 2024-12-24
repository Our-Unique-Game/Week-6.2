using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

/**
 * This component manages the goat's interactions.
 * Handles player pickup, updates allowed tiles for movement, and ensures compatibility with the game system.
 */
public class Goat : MonoBehaviour {
    [SerializeField] AllowedTiles allowedTiles = null; // Reference to the Goat's AllowedTiles
    private bool isPickedUp = false; // Tracks whether the goat is picked up.

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            OnPlayerInteraction(collision.gameObject);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) { // Debug key for testing pickup
            TogglePickup();
        }
    }

    public void OnPlayerInteraction(GameObject player) {
        if (!isPickedUp) {
            Debug.Log("Goat picked up by player!");
            isPickedUp = true;
            UpdateAllowedTilesForGoat();
        } else {
            Debug.Log("Goat is already being carried!");
        }
    }

    private void UpdateAllowedTilesForGoat() {
        if (allowedTiles != null) {
            TileBase[] goatAllowedTiles = allowedTiles.Get();

            // Get the player's AllowedTiles component
            AllowedTiles playerAllowedTiles = GameObject.FindWithTag("Player").GetComponent<AllowedTiles>();

            if (playerAllowedTiles != null) {
                playerAllowedTiles.AddAllowedTiles(goatAllowedTiles);
                Debug.Log("Goat's tiles added to player's allowed tiles.");
            } else {
                Debug.LogError("Player's AllowedTiles component is missing!");
            }
        } else {
            Debug.LogError("Goat's AllowedTiles reference is null!");
        }
    }

    public void OnPlayerDrop(GameObject player) {
        if (isPickedUp) {
            Debug.Log("Goat dropped by player.");
            isPickedUp = false;
            ResetAllowedTiles();
        }
    }

    private void ResetAllowedTiles() {
        if (allowedTiles != null) {
            // Get the player's AllowedTiles component
            AllowedTiles playerAllowedTiles = GameObject.FindWithTag("Player").GetComponent<AllowedTiles>();

            if (playerAllowedTiles != null) {
                playerAllowedTiles.ResetToDefaultTiles();
                Debug.Log("Player's allowed tiles reset to default.");
            } else {
                Debug.LogError("Player's AllowedTiles component is missing!");
            }
        } else {
            Debug.LogError("Goat's AllowedTiles reference is null!");
        }
    }

    public void TogglePickup() {
        isPickedUp = !isPickedUp;
        if (isPickedUp) {
            Debug.Log("Goat picked up via debug button.");
            UpdateAllowedTilesForGoat();
        } else {
            Debug.Log("Goat dropped via debug button.");
            ResetAllowedTiles();
        }
    }
}
