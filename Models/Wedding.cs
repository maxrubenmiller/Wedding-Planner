using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId { get;set;}
        [Required]
        [Display(Name ="Wedder One: ")]
        public string WedderOne { get;set;}
        [Required]
        [Display(Name = "Wedder Two: ")]
        public string WedderTwo{get;set;}
        [Required]
        [FutureDate]
        public DateTime Date{get;set;}
        [Required]
        public string Address {get;set;}
        public int UserId { get;set; }
        public User Planner {get;set;}
        public List<Rsvp> GuestList {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt{get;set;}

    }

    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid (object value, ValidationContext validationContext)
        {
            DateTime date;
            if(value is DateTime)
            {
                date = (DateTime)value;
            }
            else
            {
                return new ValidationResult("Invalid DateTime");
            }
            if( date < DateTime.Now)
            {
                return new ValidationResult("Date must be in the future!");
            }
            return ValidationResult.Success;
        }
    }
}