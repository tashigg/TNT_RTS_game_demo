using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : SaiSingleton<UnitSelections>
{
    [Header("Unit Selections")]
    public UnitSelectable firstUnit;
    public List<UnitSelectable> units;
    [SerializeField] protected bool isSelecting = false;
    [SerializeField] protected float isSelectTimer = 0f;
    [SerializeField] protected float isSelectLimit = 0.5f;
    [SerializeField] protected Vector3 initialMousePosition;
    [SerializeField] protected Rect selectionRect;

    private void Update()
    {
        this.ClearSelections();
        this.ChooseOneUnit();
        this.ChooseManyUnits();
    }

    public virtual void Clear()
    {
        this.units.Clear();
        this.firstUnit = null;
    }

    public virtual void Select(UnitSelectable unit)
    {
        if (!unit.unitCtrl.IsOwner()) return;

        this.Clear();
        this.units.Add(unit);
        this.firstUnit = unit;

        UnitsManager.Instance.SetCurrentBuilding(unit.unitCtrl as BuildingCtrl);
    }

    protected virtual void ClearSelections()
    {
        if (!Input.GetKeyUp(KeyCode.Escape)) return;
        this.Clear();
    }


    protected virtual void ChooseOneUnit()
    {
        if (!Input.GetKeyUp(KeyCode.Mouse0)) return;

        Ray ray = GodModeCtrl.instance._camera.ScreenPointToRay(Input.mousePosition);

        int mask = (1 << MyLayerManager.instance.layerUnitSelectable);
        if (Physics.Raycast(ray, out RaycastHit hit, 999, mask))
        {
            UnitSelectable unitSelectable = hit.collider.gameObject.GetComponent<UnitSelectable>();
            this.Select(unitSelectable);
            Debug.LogWarning(hit.collider.gameObject.transform.parent.name);
            return;
        }

        //this.Clear();
    }

    protected virtual void ChooseManyUnits()
    {
        if (this.isSelecting) this.isSelectTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            this.initialMousePosition = Input.mousePosition;
            this.isSelecting = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            this.isSelecting = false;
            this.SelectObjectsInArea();
            this.isSelectTimer = 0;
        }

        if (this.IsSelecting())
        {
            Vector3 currentMousePosition = Input.mousePosition;
            selectionRect = new Rect(
                Mathf.Min(initialMousePosition.x, currentMousePosition.x),
                Screen.height - Mathf.Max(initialMousePosition.y, currentMousePosition.y),
                Mathf.Abs(currentMousePosition.x - initialMousePosition.x),
                Mathf.Abs(currentMousePosition.y - initialMousePosition.y)
            );
        }
    }

    protected virtual bool IsSelecting()
    {
        return this.isSelectTimer > this.isSelectLimit;
    }

    void OnGUI()
    {
        if (this.IsSelecting()) GUI.Box(selectionRect, "");
    }

    void SelectObjectsInArea()
    {
        Vector3 start = Camera.main.ScreenToViewportPoint(this.initialMousePosition);
        Vector3 end = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        Rect selectionRect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);

        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.GetComponent<Renderer>() != null)
            {
                if (selectionRect.Contains(Camera.main.WorldToViewportPoint(obj.transform.position), true))
                {
                    obj.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }
}
