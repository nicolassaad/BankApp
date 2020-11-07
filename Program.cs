using System;
using System.Linq;

//TODO in general: Reduce amount of overall code
//TODO in general: Try and catch for incorrect user input being entered

// WHEN PEOPLE SHARE THE SAME NAME THE ADDRESS IS THE ONLY UNIQUE IDENTIFIER
//TODO1: Make a query that returns all accounts with the same address when you pick an account to display in Read()
//TODO2: Enable Update() to transfer money between accounts
//TODO3: Enable Read() the option to search by Account Number
//TODO Bonus: Ability to Print out the total amount of liquidity, or total cash value of all accounts. 

namespace LiteDB
{
    class MainProgram : IDisplay
    {
        public void DisplayMenu(string menuItem)
        {

        }

        public void DislayMsg(string msg)
        {
            
        }

        public int GetIntInput(int i)
        {
            return i;
        }

        public string GetStringInput(string s)
        {
            return s;
        }

        static string _strConnection="Filename=test1.litedb4; Mode=Exclusive";

        static void Main(string[] args)
        {
            while (true) {
                Console.Clear();
                DateTime now = DateTime.Now;
                Console.WriteLine("-----Welcome------------Bank-of-Merica--------Nothing-Software---v0.21");
                Console.WriteLine("------User---------------Main-Menu----------------" + now);
                
                Console.WriteLine("\nActions: S = Search   U = Update   C = Create   D = Delete   Q = Quit");
                Console.Write(">");
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

                            } else if (userInput == 'q' || userInput == 'Q')
                                {
                                    Console.WriteLine("Are you sure you want to QUIT? Y/N");
                                    char yesNo = Console.ReadKey().KeyChar;
                                    if (yesNo == 'n' || yesNo == 'N') 
                                    {
                                        Console.ReadKey();
                                    } else
                                        {
                                            Console.WriteLine("\n\nGoodbye");
                                            Console.WriteLine("Press any key...");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }

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
                Console.WriteLine("Enter Account Holder Address: ");
                string newAddress = Console.ReadLine();
                Console.WriteLine("Choose Account Type: S = Savings   C = Checking");
                char newAccountTypeChar = Console.ReadKey().KeyChar;
                string newAccountType;
                if (newAccountTypeChar == 'S' || newAccountTypeChar == 's')
                {
                    newAccountType = "Savings";
                } else
                {
                    newAccountType = "Checking";
                }
                Console.WriteLine("Type Chosen: " + newAccountType);

                //Assigns an account number by adding +1 to the total number of accounts that have ever existed preventing duplicate account numbers from being generated
                if (accounts.Count() != 0) {
                    MaxAccountNum = Math.Max(accounts.Count(), accounts.Max()); 
                }
                var account1 = new UserAccount
                {
                    AccNum = MaxAccountNum + 1,
                    AccName = newName,
                    AccValue = x,
                    AccAddress = newAddress,
                    AccType = newAccountType,
                    IsActive = true,
                };

                accounts.Insert(account1);

                Console.WriteLine("\nPress any key to go back...");
                Console.ReadKey();
            }
        }

        static void Read(string accountName)
        {
            int totalAccValue = 0;
            char accountChar = accountName[0];
            using(var db=new LiteDatabase(_strConnection))
            {
                var accounts = db.GetCollection<UserAccount>("accounts");

                //index documents using a document property
                accounts.EnsureIndex(x => x.AccName);

                var results = accounts.Query()
                    .Where(x => x.AccName.StartsWith(accountChar))
                    .OrderBy(x => x.AccName)
                    .Select(x => new { x.AccName, x.AccNum, x.AccValue, x.AccAddress, x.AccType })
                    .Limit(10)
                    .ToList();
                
                if (results != null)
                {
                    Console.WriteLine("Displaying Search Results for " + accountName);
                    Console.WriteLine("Choose one (0-9): ");

                    int count = 0;
                    foreach (var element in results) 
                    {
                        Console.WriteLine(count++ + " " + element.AccName + "  " + element.AccAddress);
                    }

                    string newValue = Console.ReadLine();
                    int n = 0;
                    Int32.TryParse(newValue, out n);
                    Console.WriteLine("\n");
                    
                    var indivResults = accounts.Query()
                        .Where(x => x.AccAddress == results[n].AccAddress.ToString() || x.AccName == results[n].AccName.ToString())
                        .OrderBy(x => x.AccName)
                        .Select(x => new { x.AccName, x.AccNum, x.AccValue, x.AccAddress, x.AccType })
                        .ToList();

                    foreach (var result in indivResults)
                    {
                        Console.WriteLine("Account Holder: " + result.AccName);
                        Console.Write("Account #: " + result.AccNum + "   ");
                        Console.Write("$" + result.AccValue + "   ");
                        Console.Write("Type: " + result.AccType + "   ");
                        Console.Write("Address: " + result.AccAddress);
                        Console.Write("\n\n");
                        totalAccValue += result.AccValue; 
                    }
                    
                    if (indivResults != null) 
                    {
                        Console.WriteLine("Total number of accounts: " + indivResults.Count());
                        Console.WriteLine("Total value for accounts: $" + totalAccValue);

                        Console.WriteLine("\nPress any key to go back..."); 
                        Console.ReadKey();
                    }  else
                    {
                        Console.WriteLine("Error Please try again");

                        Console.WriteLine("\nPress any key to go back...");
                        Console.ReadKey();
                    }

                }   else 
                    {
                        Console.WriteLine("The name, " + accountName + ", was not found in the Account Holder database.");

                        Console.WriteLine("\nPress any key to go back...");
                        Console.ReadKey();
                    }


                // if (result != null)
                // {
                //     Console.Write("Account exists: ");
                //     Console.WriteLine("Displaying results...");
                //     Console.WriteLine("Acc Holder Name: " + result.AccName);
                //     Console.WriteLine("Account #: " + result.AccNum);
                //     Console.WriteLine("Acc Value: " + result.AccValue);

                //     int n = accounts.Count();
                //     Console.WriteLine("\nCurrent Number of Active Accounts: ");

                //     Console.WriteLine("\nPress any key to go back..."); 
                //     Console.ReadKey();

                // } else 
                //     {
                //         Console.WriteLine("The name, " + accountName + ", was not found in the Account Holder database.");

                //         Console.WriteLine("\nPress any key to go back...");
                //         Console.ReadKey();
                //     }
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