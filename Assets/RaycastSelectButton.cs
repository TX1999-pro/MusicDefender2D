using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//This script casts a ray upwards from the player's position and checks if there's a button in its path. 
// If a button is found, it will be selected, and the previous button will be deselected.
public class RaycastSelectButton : MonoBehaviour
{
    public LayerMask buttonLayerMask;
    public float raycastDistance = 10f;

    private EventSystem eventSystem;
    public Button currentSelectedButton { get; private set; }

    void Start()
    {
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        CastRayUpwardsAndSelectButton();
    }

    void CastRayUpwardsAndSelectButton()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance, buttonLayerMask);
        //Debug.Log(hit.collider.name);
        if (hit.collider != null)
        {
            Button targetButton = hit.collider.GetComponent<Button>();

            if (targetButton != null && currentSelectedButton != targetButton)
            {
                if (currentSelectedButton != null)
                {
                    currentSelectedButton.OnDeselect(null);
                }

                targetButton.OnSelect(null);
                currentSelectedButton = targetButton;
            }
        }
        else if (currentSelectedButton != null)
        {
            currentSelectedButton.OnDeselect(null);
            currentSelectedButton = null;
        }
    }
}