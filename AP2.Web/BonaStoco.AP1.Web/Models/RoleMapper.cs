using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BonaStoco.AP1.Web.Models
{
    public class RoleMapper
    {
        public RoleMapper() { }
        public RoleMapper(int categoryId, int bandaraId, int terminalId, int subTerminalId,
            int role, string name, string homePage)
        {
            this.CategoryId = categoryId;
            this.BandaraId = bandaraId;
            this.TerminalId = terminalId;
            this.SubTerminalId = subTerminalId;
            this.Role = role;
            this.Name = name;
            this.HomePage = homePage;
        }

        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int BandaraId { get; set; }
        public int TerminalId { get; set; }
        public int SubTerminalId { get; set; }
        public int Role { get; set; }
        public string Name { get; set; }
        public string HomePage { get; set; }
        public string Note { get; set; }
    }
}