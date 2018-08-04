using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This is the base for mouse controls, if you click what happens and shooting rays
/// and if there is viable unit it is selected
/// </summary>
public class MouseController : MonoBehaviour {

    public Camera activeCamera; // the camera where rays are shot

    public GameObject moveIconPrefab;

    public LayerMask selectMask;

    public LayerMask moveMask;

    private bool isUnitSelected; // for quick and dirty checking if player has selected a unit

    private GameObject currentSelectedUnit; // for current selected unit



	// Use this for initialization
	void Start () {
        if (moveIconPrefab != null)
        {
            moveIconPrefab.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            ShootRays(0);
        }

        if (Input.GetMouseButtonDown(1))
        {
            ShootRays(1);
        }

    }
   
    // make the raycast for selection
    void ShootRays(int buttonNumber)
    {
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (buttonNumber == 0 && Physics.Raycast(ray, out hit, 50, selectMask))
        {
            if (currentSelectedUnit == null)
                SelectUnit(hit.collider.gameObject);
            else
            {
                DeselectUnit();
                SelectUnit(hit.collider.gameObject);
            }
        }
        else if (buttonNumber == 1 && isUnitSelected && Physics.Raycast(ray, out hit, 50, moveMask))
        {

            ShowMoveIcon(hit.point);
            MoveUnit(hit.point);
        }
        else
        {
            if(currentSelectedUnit!=null && !currentSelectedUnit.GetComponent<BaseUnit>().sprayWater)
                DeselectUnit();

        }
    }

    void ShowMoveIcon(Vector3 position)
    {
        moveIconPrefab.SetActive(true);
        moveIconPrefab.transform.position = position;
        StartCoroutine(DeactivateMoveIcon());
    }

    // For what happens when player selects unit
    void SelectUnit(GameObject unit)
    {
        if (currentSelectedUnit != null)
        {
            currentSelectedUnit.GetComponent<BaseUnit>().isSelected = false;
        }

        currentSelectedUnit = unit.gameObject;
        isUnitSelected = true;

        if (currentSelectedUnit.GetComponent<BaseUnit>() != null)
        {
            currentSelectedUnit.GetComponent<BaseUnit>().isSelected = isUnitSelected;
        }
        // TODO: Add UI elements to be activated when selected, and maybe make them to face camera
        // Also sound effects
    }

    // here we deselect the Unit
    void DeselectUnit()
    {
        if (currentSelectedUnit.GetComponent<BaseUnit>() != null)
        {
            currentSelectedUnit.GetComponent<BaseUnit>().isSelected = false;

        }
        currentSelectedUnit = null;
        isUnitSelected = false;
    }
    // function to give target destination for the Unit's NavMesh component
    void MoveUnit(Vector3 destination)
    {
        BaseUnit agent = currentSelectedUnit.GetComponent<BaseUnit>();
        if (agent != null)
        {
            agent.Move(destination);
        }
    }

    IEnumerator DeactivateMoveIcon()
    {
        yield return new WaitForSeconds(1.5f);

        moveIconPrefab.SetActive(false);
    }
}
