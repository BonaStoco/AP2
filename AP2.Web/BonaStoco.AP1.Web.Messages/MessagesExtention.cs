using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    public static class MessagesExtention
    {
        public static string ToTypeId(this Type t)
        {
            string typeId = string.Format("{0}, {1}",
                t.FullName,
                t.Assembly.GetName().Name);
            return typeId;
        }
    }
}