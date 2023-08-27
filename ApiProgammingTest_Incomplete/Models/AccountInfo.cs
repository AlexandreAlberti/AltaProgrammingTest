using System;
using System.Collections.Generic;

namespace ApiProgrammingTest
{
    public class AccountInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime SignUpTime { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public List<int> Purchases { get; set; }
    }
}