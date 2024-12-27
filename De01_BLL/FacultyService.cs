using De01.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De01_BLL
{
    public class FacultyService
    {
        public List<Lop> GetAll()
        {
            StudentContextDB context = new StudentContextDB();
            return context.Lops.ToList();
        }
    }
}
