using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonnelTracking.ViewModel
{
    public class ImageViewModel
    {

        public string ImagePath { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
    }
}