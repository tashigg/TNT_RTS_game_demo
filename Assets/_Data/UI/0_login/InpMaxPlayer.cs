using UnityEngine;

public class InpMaxPlayer : BaseInputField
{
    [Header("Max Player")]
    [SerializeField] protected int max = 16;
    [SerializeField] protected int min = 1;

    protected override void onChanged(string value)
    {
        if (value == "") return;

        int number = int.Parse(value);
        if (number > this.max) number = this.max;
        if (number < this.min) number = this.min;
        LobbyManager.Instance.maxPlayers = number;
        this.inputField.text = number.ToString();
    }
}
