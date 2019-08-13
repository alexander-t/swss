using System;
using Flying;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void NamedObjectAction(string containerName);
    public delegate void ShipAction(Ship ship);
    public delegate void TwoFloatsAcion(float firstValue, float secondValue);

    public static event NamedObjectAction onShipInspected;
    public static event NamedObjectAction onShipHit;
    public static event NamedObjectAction onShipDestroyed;
    public static event ShipAction onNewShipEntered;
    public static event TwoFloatsAcion onPlayerSpeedChanged;

    public static void RaiseShipInspected(string name)
    {
        onShipInspected?.Invoke(name);
    }

    public static void RaiseShipHit(string name)
    {
        onShipHit?.Invoke(name);
    }

    public static void RaiseShipDestroyed(string name)
    {
        onShipDestroyed?.Invoke(name);
    }

    public static void RaiseNewShipEntered(Ship ship)
    {
        onNewShipEntered?.Invoke(ship);
    }

    public static void RaisePlayerSpeedChanged(float speed, float maxSpeed) {
        onPlayerSpeedChanged(speed, maxSpeed);
    }
}
