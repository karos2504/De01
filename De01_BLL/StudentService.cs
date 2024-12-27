using De01.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace De01_BLL
{
    public class StudentService
    {
        private readonly StudentContextDB context;

        public StudentService()
        {
            context = new StudentContextDB();
        }

        public List<Sinhvien> GetAllStudents()
        {
            return context.Sinhviens.ToList();
        }

        public Sinhvien GetStudentById(string id)
        {
            return context.Sinhviens.FirstOrDefault(s => s.MaSV == id);
        }

        public void AddStudent(Sinhvien student)
        {
            context.Sinhviens.Add(student);
            context.SaveChanges();
        }

        public void UpdateStudent(Sinhvien student)
        {
            var existingStudent = context.Sinhviens.FirstOrDefault(s => s.MaSV == student.MaSV);
            if (existingStudent != null)
            {
                existingStudent.HoTenSV = student.HoTenSV;
                existingStudent.NgaySinh = student.NgaySinh;
                existingStudent.MaLop = student.MaLop;

                context.SaveChanges();
            }
        }

        public void DeleteStudent(string id)
        {
            var student = context.Sinhviens.FirstOrDefault(s => s.MaSV == id);
            if (student != null)
            {
                context.Sinhviens.Remove(student);
                context.SaveChanges();
            }
        }

        public List<Lop> GetAllClasses()
        {
            return context.Lops.ToList();
        }
    }
}
