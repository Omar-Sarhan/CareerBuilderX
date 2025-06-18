using CareerBuilderX.DTOs;
using CareerBuilderX.Models;

namespace CareerBuilderX.Repository.Core
{
    public interface IResumeRepository
    {
        List<Resume> GetAllResumesByUser(string userId); 

        Resume GetResumeById(int ResumeId);

        void  CreateResume (Resume resume);

        void UpdateResume (Resume resume);

        void DeleteResume (int ResumeId);

        int NumberOfResumes();
        int NumberOfUsers();
        List<ResumeCountByDateDTO> NumberOfResumesByDate();



    }
}
