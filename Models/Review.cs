using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EscapeRoom.Models
{
    public class Review 
    {
       private int _id;
       private int _userId;
       private int _roomId;
       //private int _rating; // 1-5
       private string _comment;
       private DateTime _createdAt;

        //referencje
        private User _user;
        private Room _room;

    }
}