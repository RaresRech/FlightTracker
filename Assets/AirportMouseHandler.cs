using UnityEngine;

public class AirportMouseHandler : MonoBehaviour
{
    private AirportPlacement airportPlacement;
    private string airportName;

    public void Initialize(AirportPlacement placement, string name)
    {
        airportPlacement = placement;
        airportName = name;
    }

    private void OnMouseEnter()
    {
        airportPlacement.OnAirportMouseEnter(airportName);
    }

    private void OnMouseExit()
    {
        airportPlacement.OnAirportMouseExit();
    }
}
