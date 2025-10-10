using PRN212_Group4.BLL;
using System;
using System.Collections.Generic;


namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            UserService service = new();
            List<PRN212_Group4.DAL.Entities.User> users = service.GetAllUsers();
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Name: {user.FullName}, Email: {user.Email}, Total Credit: {user.TotalCredit}");
            }
        }
    }
}
