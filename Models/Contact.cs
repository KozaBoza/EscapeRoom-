using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EscapeRoom.Models
{
    public class Contact
    {
        private int _id;
        private string _fullName;
        private string _email;
        private string _message;

        public int Id => _id;
        public string FullName => _fullName;
        public string Email => _email;
        public string Message => _message;

        public Contact(string fullName, string email, string message)
        {
            _id = new Random().Next(1000, 9999); // lub ustawiane z zewnątrz
            _fullName = fullName;
            _email = email;
            _message = message;
        }

        //konstruktor
        public Contact() { }
    }
}
