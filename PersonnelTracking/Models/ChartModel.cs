﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kod_Yazilim_Personel.Models
{
    public class ChartModel
    {
        public string IsimSoyisim { get; set; }
        public int? CikisSayisi { get; set; }

        public double? CalismaSaati { get; set; }

        public double? MolaSaati { get; set; }
    }
}