using System;

namespace GigHub.Web.Dtos
{
    public class GigDto
    {
        public string Id { get; set; }
        public bool IsCancelled { get; set; }
        public UserDto Artist { get; set; }
        public DateTime DateTime { get; set; }
        public string Venue { get; set; }
        public GenreDto Genre { get; set; }
    }
}