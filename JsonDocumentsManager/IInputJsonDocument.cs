namespace JsonDocumentsManager;

public interface IInputJsonDocument
{
    public abstract string GetStringData(string jsonPath);

    public abstract bool? GetBoolData(string jsonPath);
}