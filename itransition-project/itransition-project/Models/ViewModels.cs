using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace itransition_project.Models
{
    public class ComixViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Page> Pages { get; set; }
        public List<Tag> Tags { get; set; }
        public Rating Rating { get; set; }
        public AgeRating AgeRating { get; set; }
        public ApplicationUser Author { get; set; }
    }

    public class AuthorViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}