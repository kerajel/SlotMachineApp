namespace SlotMachineApp.Model;

public interface INoteSetProvider
{
    NoteSet GetNoteSet(string name);
}