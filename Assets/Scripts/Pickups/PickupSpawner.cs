using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{

    [SerializeField] EventManager eventManager;

    [SerializeField] PickUp[] pickUps;

    public List<GameObject> spawnedPickUps;

    public List<GameObject> SpawnedPickUps
    {
        get { return spawnedPickUps; }
    }


    private void OnEnable()
    {
        eventManager.OnUnitDestroyed += SpawnPickup;
    }

    private void OnDisable()
    {
        eventManager.OnUnitDestroyed -= SpawnPickup;
    }

    private void SpawnPickup(UnitType unitType, Vector3 position)
    {
        if(unitType != UnitType.Player) 
        {
            int randomIndex = Random.Range(0, pickUps.Length);
            PickUp currentPickup;

            // spawnedPickUps.Add(Instantiate<PickUp>(pickUps[randomIndex], position, Quaternion.identity));
            currentPickup = Instantiate(pickUps[randomIndex], position, Quaternion.identity);

            if(currentPickup != null) 
            { 
                spawnedPickUps.Add(currentPickup.gameObject);
            }
        }
    }

}
