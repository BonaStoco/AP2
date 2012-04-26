using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace BonaStoco.AP1.Web.Models
{
    public class RoleMaperSettingModel
    {
        public Category Category { get; set; }
        public Location Location { get; set; }
        public Terminal Terminal { get; set; }
        public SubTerminal SubTerminal { get; set; }
        public UserRole Role { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public IEnumerable<Terminal> Terminals { get; set; }
        public IEnumerable<SubTerminal> SubTerminals { get; set; }
        public IEnumerable<UserRole> Roles { get; set; }
    }

    public class Category
    {
        public int Id{get;set;}
        public string Name{get;set;}
    }
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Location(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
    public class Terminal
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Terminal(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
    public class SubTerminal
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SubTerminal(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserRole(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}