using UnityEngine;

/**
 * This component manages interactions between the player and items like the goat and boat.
 * It coordinates updates to the allowed tiles and ensures proper state management.
 */
public class InteractionManager : MonoBehaviour {
    [SerializeField] AllowedTiles allowedTiles = null;
    [SerializeField] GameObject player = null;

    private GameObject carriedItem = null; // Tracks the currently carried item.

    /**
     * Handles player interaction with an interactive object.
     * @param interactable The GameObject being interacted with (e.g., boat or goat).
     */
    public void OnPlayerInteraction(GameObject interactable) {
        if (carriedItem == null) {
            PickUpItem(interactable);
        } else if (carriedItem == interactable) {
            DropItem();
        } else {
            Debug.Log("Player is already carrying an item!");
        }
    }

    /**
     * Handles the logic for picking up an item.
     * @param item The GameObject being picked up.
     */
    private void PickUpItem(GameObject item) {
        carriedItem = item;

        if (item.TryGetComponent<Boat>(out Boat boat)) {
            boat.OnPlayerInteraction(player);
        } else if (item.TryGetComponent<Goat>(out Goat goat)) {
            goat.OnPlayerInteraction(player);
        }

        Debug.Log($"{item.name} picked up by player.");
    }

    /**
     * Handles the logic for dropping the currently carried item.
     */
    private void DropItem() {
        if (carriedItem != null) {
            if (carriedItem.TryGetComponent<Boat>(out Boat boat)) {
                boat.OnPlayerDrop(player);
            } else if (carriedItem.TryGetComponent<Goat>(out Goat goat)) {
                goat.OnPlayerDrop(player);
            }

            Debug.Log($"{carriedItem.name} dropped by player.");
            carriedItem = null;
        } else {
            Debug.Log("No item to drop!");
        }
    }

    /**
     * Checks if the player is currently carrying an item.
     * @return True if the player is carrying an item, otherwise false.
     */
    public bool IsCarryingItem() {
        return carriedItem != null;
    }

    /**
     * Gets the currently carried item.
     * @return The GameObject being carried by the player, or null if none.
     */
    public GameObject GetCarriedItem() {
        return carriedItem;
    }
}
