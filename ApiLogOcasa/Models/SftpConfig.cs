﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLogOcasa.Models
{
    public class SftpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}