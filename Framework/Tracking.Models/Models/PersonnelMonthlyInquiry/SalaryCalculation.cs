using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracking.Models.Models.PersonnelMonthlyInquiry
{
    public class SalaryCalculation
    {

        public TimeSpan CalismaZamani { get; set; } 

        public List<SalaryCalculaterDetail> salaryCalculationLists { get; set; }

    }

    public class SalaryCalculaterDetail
    {
        public string PAdSoyad { get; set; }

        public double dailywages { get; set; }

        public double NetPrice { get; set; }

    }

}
