using UnityEngine;

public abstract class LobbyManagerAbstract : SaiMonoBehaviour
{
    [Header("Lobby Manager Abstract")]
    public LobbyManager lobbyManager;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadLobbyManager();
    }

    protected virtual void LoadLobbyManager()
    {
        if (this.lobbyManager != null) return;
        this.lobbyManager = transform.parent.GetComponent<LobbyManager>();
        Debug.LogWarning(transform.name + ": LoadLobbyManager", gameObject);
    }
}
