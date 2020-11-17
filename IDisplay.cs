interface IDisplay 
{
    Actions GetAction();
    void HandleAction(IDisplay display, Actions action);
    bool IsOpen(bool boolVal);
    void Close();
}