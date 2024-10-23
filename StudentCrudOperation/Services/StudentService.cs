using StudentCrudOperation.Interface;
using StudentCrudOperation.Models;

namespace StudentCrudOperation.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<Student> AddStudent(Student student)
        {
           return await _studentRepository.AddStudent(student);
        }

        public async Task<bool> DeleteStudent(int Id)
        {
           return await _studentRepository.DeleteStudent(Id);
        }

        public async Task<Student> GetStudentById(int Id)
        {
           return await _studentRepository.GetStudentById(Id);
        }

        public async Task<List<Student>> GetStudentList()
        {
            return await _studentRepository.GetStudentList();
        }

        public async Task<Student> UpdateStudent(int id, Student student)
        {
          return await _studentRepository.UpdateStudent(id, student);
        }
    }
}
