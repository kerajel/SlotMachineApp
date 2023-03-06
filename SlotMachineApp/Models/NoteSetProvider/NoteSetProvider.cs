namespace SlotMachineApp.Model;

public class NoteSetProvider : INoteSetProvider
{
    private readonly Dictionary<string, NoteSet> _noteSets;

    public NoteSetProvider(IEnumerable<NoteSet> noteSets)
    {
        _noteSets = noteSets.ToDictionary(r => r.Name, r => r);
    }

    public NoteSet GetNoteSet(string name)
    {
        return _noteSets[name];
    }
}
