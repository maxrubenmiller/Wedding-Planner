using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class Rsvp
    {
        public int RsvpId {get;set;}
        public int UserId {get;set;}
        public int WeddingId {get;set;}
        public User Guest {get;set;}
        public Wedding Attending {get;set;}


    }
}