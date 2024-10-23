using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCrudOperation.Data;
using StudentCrudOperation.Interface;
using StudentCrudOperation.Models;

namespace StudentCrudOperation.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context) { 
        
            _context = context;
        }
        public async Task<Student> AddStudent(Student student)
        {
            var result = new Student() { 
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                Address = student.Address,
                DOB = student.DOB,
                STD = student.STD,
                CreateDate = DateTime.Now,
                ImageFile = student.ImageFile,
            };
            _context.Add(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<bool> DeleteStudent(int Id)
        {
            var student = await _context.students.FirstOrDefaultAsync(r => r.Id == Id);
            if (student != null) { 
                _context.students.Remove(student);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Student> GetStudentById(int Id)
        {
            return await _context.students.FirstOrDefaultAsync(r => r.Id == Id);             
        }

        public async Task<List<Student>> GetStudentList()
        {
            return await _context.students.ToListAsync();
        }

        public async Task<Student> UpdateStudent(int id,Student student)
        {
           var existingStudent = await _context.students.FindAsync(id);
            if (existingStudent != null) {

                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
                existingStudent.Email = student.Email;
                existingStudent.Phone = student.Phone;
                existingStudent.Address = student.Address;
                existingStudent.DOB = student.DOB;
                existingStudent.STD = student.STD;
                existingStudent.ImageFile = student.ImageFile;
                await _context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }
    }
}
