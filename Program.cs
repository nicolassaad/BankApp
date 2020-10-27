using System;
using System.Linq;

//TODO in general: Reduce amount of overall code
//TODO in general: Try and catch for incorrect user input being entered

//TODO1: Users can have more than one account. Edit search query so it can return all of a client's accounts (does this already work?)
//TODO2: Enable Update() to transfer money between accounts
//TODO3: Enable Read() the option to search by Account Number
//TODO Bonus: Ability to Print out the total amount of liquidity, or total cash value of all accounts. 

namespace LiteDB
{
    class MainProgram
    {
        static string _strConnection="Filename=test1.litedb4; Mode=Exclusive";
        static void Main(string[] args)
        {
            while (true) {
                Console.Clear();
                DateTime now = DateTime.Now;
                Console.WriteLine("-----Welcome------------Bank-of-Snow---------Nicolas-Saad--alpha-v0.125");
                Console.WriteLine("------User---------------Main-Menu----------------" + now);
                
                Console.WriteLine("\nActions: S = Search   U = Update   C = Create   D = Delete   Q = Quit");
                System.Console.WriteLine("\n\n");
                char userInput = Console.ReadKey().KeyChar;

                if (userInput == 's' || userInput == 'S')
                {   
                    Console.Clear();
                    Console.WriteLine("---------------------Search / Read---------------------------------");
                    Console.WriteLine("SEARCH for an existing account by Account Name:...");
                    string customerAcc = Console.ReadLine();
                    Read(customerAcc);

                } else if (userInput == 'u' || userInput == 'U')
                    {
                        Console.Clear();
                        Console.WriteLine("--------------------------Update-----------------------------------");
                        Console.WriteLine("UPDATE info for an existing account. Please Enter Account Name:...");
                        string customerAcc = Console.ReadLine();
                        Update(customerAcc);

                    } else if (userInput == 'c' || userInput == 'C')
                        {
                            Console.Clear();
                            Console.WriteLine("--------------------------Create-----------------------------------");
                            Console.WriteLine("Create a NEW account:");
                            Create(); 

                        } else if (userInput == 'd' || userInput == 'D') 
                            {
                                Console.Clear();
                                Console.WriteLine("\n--------------------------Delete-----------------------------------");
                                Console.WriteLine("Enter Account name to DELETE:... ");
                                string customerAcc = Console.ReadLine();
                                Delete(customerAcc);
                                Read(customerAcc);

                            } else 
                                {
                                    Console.WriteLine("\n\nGoodbye------------------------------------------------------------");
                                    Console.WriteLine("Press any key to quit...");
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                }
            } // end of while loop
        }

        static void Create()
        {
            using(var db=new LiteDatabase(_strConnection))
            {
                var accounts = db.GetCollection<UserAccount>("accounts");

                int MaxAccountNum = accounts.Count();
                Console.WriteLine("Enter New Account Name:... ");
                string newName = Console.ReadLine();
                Console.WriteLine("Enter Intitial Deposit Value:... ");
                string newValue = Console.ReadLine();
                int x = 0;
                Int32.TryParse(newValue, out x);
                
                if (accounts.Count() != 0) {
                    MaxAccountNum = Math.Max(accounts.Count(), accounts.Max()); 
                }
                var account1 = new UserAccount
                {
                    AccNum = MaxAccountNum + 1, //Account numbers are auto generated. I need to find the highest account number and add one to it
                    AccName = newName,
                    AccValue = x,
                    IsActive = true,
                };

                accounts.Insert(account1);
    
                Console.WriteLine("Current Number of Active Accounts: ");

                Console.WriteLine("\nPress any key to go back...");
                Console.ReadKey();
            }
        }

        static void Read(string accountName)
        {
            using(var db=new LiteDatabase(_strConnection))
            {
                var accounts = db.GetCollection<UserAccount>("accounts");

                //index documents using a document property
                accounts.EnsureIndex(x => x.AccName);
                                
                var result = accounts.Find(x => x.AccName == accountName).FirstOrDefault();

                if (result != null)
                {
                    Console.Write("Account exists: ");
                    Console.WriteLine("Displaying results...");
                    Console.WriteLine("Acc Holder Name: " + result.AccName);
                    Console.WriteLine("Account #: " + result.AccNum);
                    Console.WriteLine("Acc Value: " + result.AccValue);

                    int n = accounts.Count();
                    Console.WriteLine("\nCurrent Number of Active Accounts: " + n.ToString());

                    Console.WriteLine("\nPress any key to go back..."); 
                    Console.ReadKey();

                } else 
                    {
                        Console.WriteLine("The name, " + accountName + ", was not found in the Account Holder database.");

                        Console.WriteLine("\nPress any key to go back...");
                        Console.ReadKey();
                    }
            }
        }

        static void ClearDataBase() //Deletes entire database
        {
            using(var db=new LiteDatabase(_strConnection))
            {
                db.DropCollection("accounts");
                // db.Shrink();
            }
        }

        static void Update(string accountName)
        {
            using(var db=new LiteDatabase(_strConnection))
            {
                var accounts = db.GetCollection<UserAccount>("accounts");

                //index documents using a document property
                accounts.EnsureIndex(x => x.AccName);
                
                var result = accounts.Find(x => x.AccName == accountName).FirstOrDefault();

                if (result != null)
                {
                    Console.WriteLine("Actions:  N = Change Name for "  + result.AccName + "    V = Change Amount for Account #" + result.AccNum + "   H = Home");
                    // TODO: Ask Trevor why the ReadKey isn't working properly here
                    char input = Console.ReadKey().KeyChar;
                    if (input == 'n' || input == 'N')
                    {
                        Console.WriteLine("\nEnter new Account Name for " + result.AccName);

                        string updateName = Console.ReadLine();
                        result.AccName = updateName;

                        Console.WriteLine("Account's name has been set to " + updateName);

                    } else if (input == 'v' || input == 'V')
                        {
                            //TODO1: Add ability to transfer money between accounts. 
                            Console.WriteLine("\nCurrent Account Value.... " + result.AccValue);
                            Console.WriteLine("Enter new Value for " + result.AccName);

                            string updateValue = Console.ReadLine();

                            int x = 0;
                            Int32.TryParse(updateValue, out x);
                            result.AccValue = x;

                            Console.WriteLine("Account's name has been set to " + x);

                        } else if (input == 'h' || input == 'H')
                        {
                            Console.ReadKey();
                        }
                    accounts.Update(result);

                    Console.WriteLine("\nPress any key to go back...");
                    Console.ReadKey();

                } else 
                    {
                        Console.WriteLine("Account for the name " + accountName + " was not found.");
                        Console.WriteLine("\nPress any key to go back...");
                        Console.ReadKey();
                    }
            }
        }

        static void Delete(string accountName)
        {
            using(var db=new LiteDatabase(_strConnection))
            {
                var accounts = db.GetCollection<UserAccount>("accounts");

                //index documents using a document property
                accounts.EnsureIndex(x => x.AccName);

                var result = accounts.Find(x => x.AccName == accountName).FirstOrDefault();

                if (result != null)
                {
                    Console.WriteLine("Are you sure you want to delete " + result.AccName + "'s account? Y/N...");
                    char input = Console.ReadKey().KeyChar;
                    if (input == 'Y' || input == 'y')
                    {
                        //delete
                        var p = new BsonValue(result.Id);
                        accounts.Delete(p);
                        
                        Console.WriteLine("\nAccount has been deleted.");
                        int n = accounts.Count();
                        Console.WriteLine("Current Number of Active Accounts: " + n.ToString());

                    } else
                        {
                            Console.WriteLine("\nAccount has not been deleted.");
                            int n = accounts.Count();
                            Console.WriteLine("Current Number of Active Accounts. " + n.ToString());
                        }

                } else 
                    {
                        Console.WriteLine("Account: " + accountName + " was not found.");

                        Console.WriteLine("\nPress any key to go back...");
                        Console.ReadKey();
                    }
            }
        }
    }
}