// The IDisplay Interface handles everything that is seen onscreen, user input and program output. 

interface IDisplay
{
    void DisplayMenu(string menuItem);
    void DislayMsg(string msg);
    int GetIntInput(int i);
    string GetStringInput(string s);
}