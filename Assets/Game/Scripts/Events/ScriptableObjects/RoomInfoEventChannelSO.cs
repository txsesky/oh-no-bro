using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have one int argument.
/// Example: An Achievement unlock event, where the int is the Achievement ID.
/// </summary>

[CreateAssetMenu(menuName = "Events/RoomInfo Event Channel")]
public class RoomInfoEventChannelSO : EventChannelBaseSO
{
    public UnityAction<RoomInfo> OnEventRaised;
    public void RaiseEvent(RoomInfo value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}