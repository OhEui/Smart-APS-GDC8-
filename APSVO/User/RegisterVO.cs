﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSVO
{
    public class RegisterVO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }

    }
}
