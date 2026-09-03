using UnityEngine;

public class PotSelectionOption2 : MonoBehaviour
{
    [Header("Tree Container")]
    public GameObject treeContainer;  // Parent object holding all tree variants

    [Header("Tracking")]
    public GameObject currentTree;    // Currently visible tree (one of the base trees)

    [Header("Alternates")]
    public GameObject alternateForTreeA; // The tree to show if BaseTreeA is active
    public GameObject alternateForTreeB; // The tree to show if BaseTreeB is active

    // Call this from a button
    public void SwitchBasedOnCurrent()
    {
        // Auto-detect the currently active tree if not assigned
        if (treeContainer != null)
        {
            foreach (Transform child in treeContainer.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    currentTree = child.gameObject;
                    break;
                }
            }

            if (currentTree == null)
            {
                Debug.LogWarning("No active tree found in container. Please ensure one tree is active.");
            }
        }

        GameObject nextTree = null;

        string treeName = currentTree.name;
        string[] parts = treeName.Split('_');

        //change the tree selection based on the tree index
        if (parts.Length > 2 && int.TryParse(parts[2], out int treeNumber))
        {
            // Example condition check
            if (treeNumber == 1)
            {
                nextTree = alternateForTreeA;
            }
            else if (treeNumber == 9)
            {
                nextTree = alternateForTreeB;
            }
        }

        // Check which base tree is active
        //if (currentTree.name == "Bonsai_Tree_1_Orig")
        //{
        //    nextTree = alternateForTreeA;
        //}
        //else if (currentTree.name == "Bonsai_Tree_9_New_Pot")
        //{
        //    nextTree = alternateForTreeB;
        //}

        if (nextTree != null)
        {
            // Hide the current tree
            currentTree.SetActive(false);

            // Show the alternate tree
            nextTree.SetActive(true);

            // Update the tracker
            currentTree = nextTree;
        }
        else
        {
            Debug.LogWarning("No alternate defined for current tree: " + currentTree.name);
        }
    }
}