using System;
using System.DirectoryServices;

namespace UserHandle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter User Account Options 1 to create and 2 to remove and 3 to reset password:");
                var op= Console.ReadLine();
                Console.WriteLine("Enter user name:");
                var un = Console.ReadLine();
                if (op == "1")
                {
                    Console.WriteLine("Enter Password:");
                    var pwd = Console.ReadLine();
                    DirectoryEntry AD = new DirectoryEntry("WinNT://" +
                    Environment.MachineName + ",computer");
                    DirectoryEntry NewUser = AD.Children.Add(un, "user");
                    NewUser.Invoke("SetPassword", new object[] { pwd });
                    NewUser.Invoke("Put", new object[] { "Description", "Test User from .NET" });
                    NewUser.CommitChanges();
                    DirectoryEntry grp;

                    grp = AD.Children.Find("Guests", "group");
                    if (grp != null) { grp.Invoke("Add", new object[] { NewUser.Path.ToString() }); }
                    Console.WriteLine("Account Created Successfully");
                }
                else
                {
                    DirectoryEntry localDirectory = new DirectoryEntry("WinNT://" + Environment.MachineName.ToString());
                    DirectoryEntries users = localDirectory.Children;
                    DirectoryEntry user = users.Find(un);
                    if (op == "2")
                    {
                        users.Remove(user);
                        Console.WriteLine("Account Removed Successfully");
                    }
                    else
                    {
                        Console.WriteLine("Enter new Password:");
                        var pwd = Console.ReadLine();
                        user.Invoke("SetPassword", new object[] { pwd });
                        user.CommitChanges();
                        Console.WriteLine("Account Password Changed Successfully");
                    }
                   
                }
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
