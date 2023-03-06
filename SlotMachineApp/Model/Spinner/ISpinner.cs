namespace SlotMachineApp.Model
{
    public interface ISpinner
    {
        IEnumerator<SpinnerSection> GetSectionEnumerator();
    }
}