interface IAction : IResult
{
    IAction GetAction();
    void HandleAction(IDisplay display, IAction action);
}