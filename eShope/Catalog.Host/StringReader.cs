namespace Catalog.Host;

public class StringReader: IStringReader
{
    private static int _counter = 0;

    public StringReader()
    {
        _counter += 1;
    }
    public string ReadString()
    {
        //Represents a globally unique identifier (GUID).
        // return new Guid().NewGuid().ToString();
        return _counter.ToString();
    }
}