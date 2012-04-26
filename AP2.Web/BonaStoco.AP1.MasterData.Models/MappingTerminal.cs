using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindTerminalById",@"SELECT terminalid, terminalname, locationid FROM mappingterminal Where terminalid = @terminalid")]
    [NamedSqlQuery("FindTerminalByCategoryId", @"SELECT terminalid, terminalname,locationid From mappingterminal WHERE categoryid=@categoryid")]
    [NamedSqlQuery("FindTerminalByCategoryIdAndLocationId", @"SELECT terminalid, terminalname,locationid From mappingterminal WHERE categoryid=@categoryid AND locationid=@locationid")]
    public class MappingTerminal : IViewModel
    {
        public int TerminalId { get; set; }
        public string TerminalName { get; set; }
        public int LocationId { get; set; }
    }
}
