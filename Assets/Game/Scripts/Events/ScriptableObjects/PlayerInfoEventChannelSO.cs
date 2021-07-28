using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have one int argument.
/// Example: An Achievement unlock event, where the int is the Achievement ID.
/// </summary>

[CreateAssetMenu(menuName = "Events/PlayerInfo Event Channel")]
public class PlayerInfoEventChannelSO : EventChannelBaseSO
{
    public UnityAction<Player> OnEventRaised;
    public void RaiseEvent(Player value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}