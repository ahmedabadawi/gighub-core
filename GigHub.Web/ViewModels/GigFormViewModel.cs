using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using GigHub.Web.Models;
using GigHub.Web.Controllers;

namespace GigHub.Web.ViewModels
{
    public class GigFormViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Venue { get; set; }
        
        [Required]
        [FutureDate]
        public string Date { get; set; }
        
        [Required]
        [ValidTime]
        public string Time { get; set; }
        
        [Required]
        public byte Genre { get; set; }
        
        public IEnumerable<Genre> Genres { get; set; }
        
        public DateTime GetDateTime() 
        { 
            return DateTime.Parse(string.Format("{0} {1}", Date, Time)); 
        }

        public string Heading { get; set; }

        public string Action 
        { 
            get
            {
                Expression<Func<GigsController, Task<IActionResult>>> update = 
                    (c => c.Update(this));
                Expression<Func<GigsController, Task<IActionResult>>> create = 
                    (c => c.Create(this));
                
                var action = (!String.IsNullOrEmpty(Id)) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            } 
        }
    }
}