using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTracking : MonoBehaviour
{
    [SerializeField] LayerMask layer;

    private Grid grid;
    private Camera mainCamera;
    private StateImput stateImput = StateImput.defaultState;
    private Building flyingBuilding;
    private bool available;

    private enum StateImput
    {
        defaultState,
        building,
        cooking
    }

    private void Start()
    {
        grid = Grid.Instance;
        mainCamera = Camera.main;
        Events.CookingActivation += () => { Events.BuildingDeactivation(); stateImput = StateImput.cooking; };
        Events.CookingDeactivation += () => { stateImput = StateImput.defaultState; };
        Events.BuildingActivation += () => { Events.CookingDeactivation(); stateImput = StateImput.building; };
        Events.BuildingDeactivation += () => { stateImput = StateImput.defaultState; };
    }

    private void Update()
    {
        if (stateImput == StateImput.cooking)
            Cooking();
        else if (stateImput == StateImput.building)
            Building();
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildingPrefab);
    }

    private void Cooking()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit, 1000, layer))
        {
            grid.TransformationTile(hit.transform.GetComponent<Tile>().Coordinate);
        }
    }

    private void Building()
    {
        if (flyingBuilding != null)
        {
            ActivationTexture(flyingBuilding.buildingSite);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000, layer))
            {
                available = grid.CanBuild(hit.transform.GetComponent<Tile>().Coordinate, flyingBuilding.size, flyingBuilding.buildingSite);

                flyingBuilding.SetTransparent(available);

                Vector3 worldPosition = hit.transform.position + new Vector3((flyingBuilding.size.x - 1) / 2f, 0, (flyingBuilding.size.y - 1) / 2f); ;
                worldPosition.y += 1f;
                flyingBuilding.transform.position = worldPosition;

                if (available && Input.GetMouseButtonDown(0))
                {
                    DeactivationTexture();
                    grid.SetBuild(hit.transform.GetComponent<Tile>().Coordinate, flyingBuilding.size);

                    flyingBuilding.SetNormal();
                    Events.AddBuilding(flyingBuilding.transform);
                    flyingBuilding = null;
                }
            }
        }
    }

    private void ActivationTexture(Types.Terrain terrain)
    {
        Grid.Instance.ActivationGridTexture(terrain);
    }

    private void DeactivationTexture()
    {
        Grid.Instance.DeactivationGridTexture();
    }
}
