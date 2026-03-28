using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kinoshechka
{
    internal class Core
    {
        public static cinemaEntities Context = new cinemaEntities();

        public static int? CurrentUserId { get; set; }
    }
}
