using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterimageFX : MonoBehaviour
{
    [Header("Dependencies")]
    public SkinnedMeshRenderer characterSkinnedMesh; // Reference to the character's SkinnedMeshRenderer
    public Transform playerTransform;               // Reference to the player's transform
    public Material afterimageMaterial;             // Material for the afterimages

    [Header("Settings")]
    public Color[] mainColors;                      // List of colors to randomly apply
    public float fadeDuration = 1f;                 // How long before the afterimage disappears
    public float spawnInterval = 0.1f;              // Time between afterimage spawns
    public float travelSpeed = 3f;                  // Speed at which afterimages move
    public Transform portalPosition;                  // Position of the portal

    private float spawnTimer;                       // Timer to control spawn intervals
    public bool isActive = false;                  // Controls whether the effect is active
    private List<GameObject> activeAfterimages = new List<GameObject>(); // Track spawned afterimages

    void Update()
    {
        if (isActive)
        {
            // Count down the timer
            spawnTimer -= Time.deltaTime;

            // Spawn afterimages at the specified interval
            if (spawnTimer <= 0f)
            {
                SpawnAfterimage();
                spawnTimer = spawnInterval; // Reset the timer
            }
        }
    }

    public void StartAfterimageEffect()
    {
        isActive = true;
        spawnTimer = 0f; // Start spawning immediately
    }

    public void StopAfterimageEffect()
    {
        isActive = false;
    }

    private void SpawnAfterimage()
    {
        // Create a snapshot of the character's mesh
        Mesh snapshotMesh = new Mesh();
        characterSkinnedMesh.BakeMesh(snapshotMesh);

        // Create a new GameObject for the afterimage
        GameObject afterimageObject = new GameObject("Afterimage");
        MeshFilter afterimageMeshFilter = afterimageObject.AddComponent<MeshFilter>();
        MeshRenderer afterimageMeshRenderer = afterimageObject.AddComponent<MeshRenderer>();

        // Assign the snapshot mesh and material to the afterimage
        afterimageMeshFilter.mesh = snapshotMesh;
        afterimageMeshRenderer.material = new Material(afterimageMaterial);

        // Randomly select a main color from the defined list
        Color randomColor = mainColors[Random.Range(0, mainColors.Length)];
        afterimageMeshRenderer.material.color = randomColor;

        // Position and rotate the afterimage
        afterimageObject.transform.position = playerTransform.position;
        afterimageObject.transform.rotation = playerTransform.rotation;

        // Add the afterimage to the list
        activeAfterimages.Add(afterimageObject);

        // Move the afterimage toward the portal and fade it out
        StartCoroutine(MoveAndFadeAfterimage(afterimageObject));
    }

    private IEnumerator MoveAndFadeAfterimage(GameObject afterimageObject)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = afterimageObject.transform.position;
        Vector3 directionToPortal = (portalPosition.position - startPosition).normalized;

        // Fade and move the afterimage over time
        while (elapsedTime < fadeDuration)
        {
            float progress = elapsedTime / fadeDuration;

            // Move the afterimage toward the portal
            afterimageObject.transform.position = Vector3.Lerp(
                startPosition,
                portalPosition.position,
                progress
            );

            // Fade out the material
            Color color = afterimageObject.GetComponent<MeshRenderer>().material.color;
            color.a = Mathf.Lerp(1f, 0f, progress);
            afterimageObject.GetComponent<MeshRenderer>().material.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Remove the afterimage from the list and destroy it
        activeAfterimages.Remove(afterimageObject);
        Destroy(afterimageObject);
    }

    private void OnDisable()
    {
        // Destroy all active afterimages when the parent GameObject is deactivated
        foreach (GameObject afterimage in activeAfterimages)
        {
            if (afterimage != null)
            {
                Destroy(afterimage);
            }
        }

        // Clear the list
        activeAfterimages.Clear();
    }
}
