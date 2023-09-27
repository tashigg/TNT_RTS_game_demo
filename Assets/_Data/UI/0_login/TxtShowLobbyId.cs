
public class TxtShowLobbyId : BaseText
{

    private void FixedUpdate()
    {
        this.ShowingLobbyId();
    }

    protected virtual void ShowingLobbyId()
    {
        this.textMeshPro.text = LobbyManager.Instance.lobbyId??"no-id";
    }
}
