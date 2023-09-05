using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirportRouteMaker : MonoBehaviour
{
    [Header("Link to the Flight Tracker (Required)")]
    [SerializeField]
    private GameObject flightTracker;

    private List<string> currentRouteString;
    private Transform previouslySelectedDAirport;
    private Transform previouslySelectedAAirport;

    private Dictionary<string, Material> airportMaterials = new Dictionary<string, Material>();

    void Start()
    {
        flightTracker = GameObject.Find("Flight Tracker");
        StoreInitialMaterials();
    }

    void Update()
    {
        currentRouteString = flightTracker.GetComponent<RandomFlightInfo>().GetCurrentRoute(); // Index 0 is arrival, 1 is departure

        if (currentRouteString[0] != "")
        {
            ChangeAirportMaterials(currentRouteString[1], currentRouteString[0]);
        }
    }

    void StoreInitialMaterials()
    {
        airportMaterials.Clear();
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                airportMaterials[child.name] = renderer.material;
            }
        }
    }

    void ChangeAirportMaterials(string departure, string arrival)
    {
        // Revert previously selected airports to their initial material
        RevertAirportMaterial(previouslySelectedDAirport);
        RevertAirportMaterial(previouslySelectedAAirport);

        foreach (Transform child in transform)
        {
            if (child.name == departure || child.name == arrival)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Store the previously selected airports
                    if (child.name == departure)
                    {
                        previouslySelectedDAirport = child;
                    }
                    else if (child.name == arrival)
                    {
                        previouslySelectedAAirport = child;
                    }

                    // Change the material to yellow with high emission
                    Material airportMaterial = new Material(renderer.material);
                    airportMaterial.color = Color.yellow;
                    airportMaterial.EnableKeyword("_EMISSION");
                    airportMaterial.SetColor("_EmissionColor", Color.yellow * 2.0f); // Adjust emission intensity
                    renderer.material = airportMaterial;
                }
            }
        }
    }

    void RevertAirportMaterial(Transform airport)
    {
        if (airport != null)
        {
            Renderer renderer = airport.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material initialMaterial;
                if (airportMaterials.TryGetValue(airport.name, out initialMaterial))
                {
                    renderer.material = initialMaterial;
                }
            }
        }
    }
}
