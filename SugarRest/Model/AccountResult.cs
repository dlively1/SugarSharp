using System;
using System.Collections.Generic;

namespace SugarRest.Model
{
    public class AccountResult : BeanListBase
    {
        public List<Account> records { get; set; }
    }
}
