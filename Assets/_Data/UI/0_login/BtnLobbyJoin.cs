
public class BtnLobbyJoin : BaseButton
{

    private void FixedUpdate()
    {
        this.CheckLobbyCreated();
    }

    protected virtual void CheckLobbyCreated()
    {
        if (this.button.interactable == false) return;
        if (!LobbyManager.Instance.isLobbyHost) return;
        this.button.interactable = false;
    }

    protected override void OnClick()
    {
        LobbyManager.Instance.JoinLobby();
    }
}
