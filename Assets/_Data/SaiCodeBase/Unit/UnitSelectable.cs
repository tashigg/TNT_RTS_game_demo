public class UnitSelectable : UnitAbstract
{
    public virtual bool IsSelected()
    {
        return UnitSelections.Instance.Contains(this);
    }
}
