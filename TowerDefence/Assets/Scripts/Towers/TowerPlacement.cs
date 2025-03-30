using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask placementCollideMask;
    [SerializeField] private LayerMask placementCheckMask;
    public static GameObject unit;
    public static bool hasBeenPlaced;
    private int totalUnits = 0;
    
    
    // Update is called once per frame
    void Update()
    {
        if (unit != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ObjectPoolManager.ReturnObjectToPool(unit);
                Debug.Log("Placement cancelled.");
                unit = null;
                return; 
            }
            
            IStats unitToSpawn = unit.GetComponent<IStats>();
            hasBeenPlaced = false;
            Ray r = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            
            if (Physics.Raycast(r, out  hitInfo, Mathf.Infinity , placementCollideMask))
            {
                unit.transform.position = hitInfo.point;
            }
            
            if (Input.GetMouseButtonDown(0) && hitInfo.collider.gameObject != null)
            {
                if (unitToSpawn.CanSpawn())
                {
                    if (unit.CompareTag("WallUnit"))
                    {
                        WallUnitPlacement(hitInfo);
                    }
                    else if (unit.CompareTag("FloorUnit"))
                    {
                        FloorUnitPlacement(hitInfo);
                    }
                }
                else
                {
                    Debug.Log("not enough resources");
                }
            }
        }
    }
    

    private void WallUnitPlacement(RaycastHit hitInfo)
    {
        if (hitInfo.collider.gameObject.CompareTag("CanPlaceWallUnit"))
        {
            BoxCollider unitCollider = unit.gameObject.GetComponent<BoxCollider>();
            unitCollider.isTrigger = true;
            IUnitStats unitPlacementCheck = unit.GetComponent<IUnitStats>();
                    
            Vector3 BoxCenter = unit.gameObject.transform.position + unitCollider.center;
            Vector3 HalfExtents = unitCollider.size / 2;
            
            if (Physics.CheckBox(BoxCenter, HalfExtents, Quaternion.identity, placementCheckMask, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("can't place here");
                unitCollider.isTrigger = true;
            }
            else
            {
                hasBeenPlaced = true;
                if (hasBeenPlaced)
                {
                    unit.layer = LayerMask.NameToLayer("Towers");
                    Debug.Log("placed");
                    totalUnits++;
                    UnitTracker.currentUnitsSpawned = totalUnits;
                    unitCollider.isTrigger = false;
                    unitPlacementCheck.OnPlacement();
                    unit = null;
                }
            }
        }
    }
    
    private void FloorUnitPlacement(RaycastHit hitInfo)
    {
        if (hitInfo.collider.gameObject.CompareTag("CanPlaceFloorUnit"))
        {
            BoxCollider unitCollider = unit.gameObject.GetComponent<BoxCollider>();
            unitCollider.isTrigger = true;
            IUnitStats unitPlacementCheck = unit.GetComponent<IUnitStats>();
                    
            Vector3 BoxCenter = unit.gameObject.transform.position + unitCollider.center;
            Vector3 HalfExtents = unitCollider.size / 2;
                    
                    
            if (Physics.CheckBox(BoxCenter, HalfExtents, Quaternion.identity, placementCheckMask, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("can't place here");
                unitCollider.isTrigger = true;
            }
            else
            {
                hasBeenPlaced = true;
                if (hasBeenPlaced)
                {
                    unit.layer = LayerMask.NameToLayer("Towers");
                    Debug.Log("placed");
                    totalUnits++;
                    UnitTracker.currentUnitsSpawned = totalUnits;
                    unitCollider.isTrigger = false;
                    unitPlacementCheck.OnPlacement();
                    unit = null;
                }
            }
        }
    }
    
    public void UnitToPlace(GameObject selectedUnit)
    {
        unit = ObjectPoolManager.SpawnObject(selectedUnit, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.playerUnits);
        unit.layer = LayerMask.NameToLayer("Temporary");
    }
}
