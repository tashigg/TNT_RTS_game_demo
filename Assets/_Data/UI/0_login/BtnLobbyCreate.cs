public class BtnLobbyCreate : BaseButton
{

    private void FixedUpdate()
    {
        this.CheckLobbyJoined();
    }

    protected virtual void CheckLobbyJoined()
    {
        if (this.button.interactable == false) return;
        if (!LobbyManager.Instance.isJoinedLobby) return;
        this.button.interactable = false;
    }

    protected override void OnClick()
    {
        LobbyManager.Instance.CreateLobby();
    }
}
