using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Car_Dealership.Models
{
    public class Attributes
    {
         public class RoleAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string role = value.ToString();
                if (role == "admin" || role == "user")
                {
                    return true;
                }
                else
                {
                    this.ErrorMessage = "Role must be admin or user!";
                }
            }

            return false;
        }
    }

    public class YearAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string tmp = value.ToString();
                int year = Int32.Parse(tmp);
                if (year >= 1800 && year <= 2020)
                {
                    return true;
                }
                else
                {
                    this.ErrorMessage = "Year must be between 1800-2020!";
                }
            }

            return false;
        }
    }
    public class CapacityAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string tmp = value.ToString();
                int capacity = Int32.Parse(tmp);
                if (capacity > 0 && capacity <= 50)
                {
                    return true;
                }
                else
                {
                    this.ErrorMessage = "Capacity must be between 0-50!";
                }
            }

            return false;
        }
    }
    public class PasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string password = value.ToString();
                string pattern = "^[a-zA-Z0-9.!#$%&''*+/=?^_`{|}~-]+$";
                if (Regex.IsMatch(password, pattern))
                {
                    return true;
                }
                else
                {
                    this.ErrorMessage =
                        "Password must have upper and lower case characters, numbers and special characters!";
                }
            }

            return false;
        }
    }
    }
}