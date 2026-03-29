
using UnityEngine;

public class FishInteractable : MonoBehaviour
{
/* Attach this to each NPC fish in the scene.
-  When the player swims into it, they "become" that fish:
- discovery is logged
- the info popup fires
- the player's mesh swaps to this fish's appearance
*/   

    public fishData data;

    //  player's renderer will swap to new mesh on contact.
    public Mesh fishMesh;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        FishDiscoveryManager.Instance.Discover(data);

        MeshFilter playerMesh = other.GetComponent<MeshFilter>();
        if (playerMesh != null && fishMesh != null)
            playerMesh.mesh = fishMesh;

        Destroy(gameObject);
    }
}

/*NOTES
remember to tag  player GameObject "Player" in  Inspector
onTriggerEnter: logs the discovery (popup + counter update happen inside Discover())
swaps the player's visible mesh to look like new fish
now that player has become new fishmesh, destroy the old one to avoid repeat discoveries
*/

