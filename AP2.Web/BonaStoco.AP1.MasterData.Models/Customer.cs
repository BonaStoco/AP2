﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [SqlQuery("SELECT * from cargocustomer")]
    [Serializable]
    public class Customer : IViewModel
    {
        public Guid Guid { get; set; }
        public string Kode { get; set; }
        public string Nama { get; set; }
    }
}
