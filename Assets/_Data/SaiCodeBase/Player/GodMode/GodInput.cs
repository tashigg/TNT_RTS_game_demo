using UnityEngine;

public class GodInput : SaiMonoBehaviour
{
    public GodModeCtrl godModeCtrl;
    public bool isMouseRightClick = false;
    public bool isMouseRotating = false;
    public Vector2 mouseScroll = new Vector2();
    public Vector3 mouseReference = new Vector3();
    public Vector3 mouseRotation = new Vector3();

    protected void Update()
    {
        this.InputHandle();
        this.MouseRotation();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGetModeCtrl();
    }

    protected virtual void LoadGetModeCtrl()
    {
        if (this.godModeCtrl != null) return;
        this.godModeCtrl = GetComponent<GodModeCtrl>();
        Debug.Log(transform.name + ": LoadGetModeCtrl", gameObject);
    }

    protected virtual void InputHandle()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = Input.mouseScrollDelta.y * -1;
        bool leftShift = Input.GetKey(KeyCode.LeftShift);

        this.godModeCtrl.godMovement.camMovement.x = x;
        this.godModeCtrl.godMovement.camMovement.z = z;
        this.godModeCtrl.godMovement.camMovement.y = y;
        this.godModeCtrl.godMovement.speedShift = leftShift;
    }

    protected virtual void MouseRotation()
    {
        this.isMouseRightClick = Input.GetKey(KeyCode.Mouse1);
        if (Input.GetKeyDown(KeyCode.Mouse1)) this.mouseReference = Input.mousePosition;

        if (this.isMouseRightClick) this.CheckMouseRotation();
        else this.mouseRotation = Vector3.zero;

        this.godModeCtrl.godMovement.camRotation.y = this.mouseRotation.x;
    }

    protected virtual void CheckMouseRotation()
    {
        this.mouseRotation = (Input.mousePosition - this.mouseReference);
        this.mouseRotation.y = -(this.mouseRotation.x + this.mouseRotation.y);
        this.mouseReference = Input.mousePosition;
        if (this.mouseRotation.x == 0 && this.mouseRotation.y == 0) return;
        if (this.isMouseRotating == false) CheckMouseRotating();
        this.isMouseRotating = true;
    }

    protected virtual void CheckMouseRotating()
    {
        if (this.mouseRotation.x != 0
            || this.mouseRotation.y != 0
            || this.isMouseRightClick)
        {
            Invoke(nameof(this.CheckMouseRotating), 0.4f);
            return;
        }

        this.isMouseRotating = false;
    }
}
