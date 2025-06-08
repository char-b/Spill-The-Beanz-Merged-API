using Microsoft.EntityFrameworkCore;
using Spill_The_Beanz_Coffee_Shop_API.Models;
using System;
using BCrypt.Net;
using System.IO;


namespace Spill_The_Beanz_Coffee_Shop_API.Services
{
    public class AdminService
    {
        public static void CreateAdmin() //any other way to improve security?
        {
            //you can change these obv. 
            string password = "thisIsAPlainPassword!";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            File.AppendAllText("hashed_password.txt", hashedPassword);
            

            string password2 = "12345!";
            string hashedPassword2 = BCrypt.Net.BCrypt.HashPassword(password);
            File.AppendAllText("hashed_password.txt", hashedPassword2 + Environment.NewLine);

            //The commented code below is just to check if the the entered password matches the hash. not needed

            //string entered = "12345!";
            //string storedHash = "$2a$11$S4BEUhINC85G4gUOFZbf4eeXZwDadXUkzTYjyeB.ez8uoEAL2KqYi";
            //bool match = BCrypt.Net.BCrypt.Verify(entered, storedHash);
            //string result = match.ToString();
            //File.AppendAllText("hashed_password.txt", result);  // Should say true


        }

    }
}
