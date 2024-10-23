using StudentCrudOperation.Models;

namespace StudentCrudOperation.Interface
{
    public interface IStudentService
    {
        public Task<Student> AddStudent(Student student);
        public Task<Student> UpdateStudent(int id, Student student);
        public Task<bool> DeleteStudent(int Id);
        public Task<Student> GetStudentById(int Id);
        public Task<List<Student>> GetStudentList();
    }
}
