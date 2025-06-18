using CareerBuilderX.Data;
using CareerBuilderX.DTOs;
using CareerBuilderX.Models;
using CareerBuilderX.Repository.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;



namespace CareerBuilderX.Repositories.Repo
{
    public class ResumeRepository : IResumeRepository 
    {

        private ApplicationDbContext _context { get; set; }

        public ResumeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public List<Resume> GetAllResumesByUser(string userId)
        {
            return _context.Resumes
    .Where(r => r.EndUserId == userId && !r.IsDeleted)
    .ToList();

        }


        public Resume GetResumeById(int ResumeId)
        {

            return _context.Resumes
                .Include(r => r.Educations)
                .Include(r => r.Experiences)
                .Include(r => r.Skills)
                .Include(r => r.Certifications)
                .Include(r => r.Languages)
                .SingleOrDefault(r => r.ResumeId == ResumeId);
        }
        
        public void CreateResume(Resume resume)
        {
            _context.Resumes.Add(resume);
            _context.SaveChanges();
        }
        public void UpdateResume(Resume resume)
        {
            _context.Resumes.Update(resume);
            _context.SaveChanges();
        }

        public void DeleteResume(int ResumeId)
        {
            var resume = GetResumeById(ResumeId);
            if (resume != null) 
            {

                resume.IsDeleted = true;

            }
            _context.SaveChanges();
        }

        public int NumberOfResumes()
        {
            int Count =  _context.Resumes
        .Where(r => r.IsDeleted == false)
        .ToList().Count;
            return Count;
        }

        public int NumberOfUsers() 
        {
            int Count = _context.EndUsers
        .ToList().Count;
            return Count;
        }

        public List<ResumeCountByDateDTO> NumberOfResumesByDate()
        {
            var today = DateTime.Today;
            var fromDate = today.AddDays(-30);

            var result = _context.Resumes
                .Where(r => !r.IsDeleted && r.CreatedDate.Date >= fromDate)
                .AsEnumerable()
                .GroupBy(r => r.CreatedDate.Date)
                .Select(g => new ResumeCountByDateDTO
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(r => r.Date)
                .ToList();



            return result;
        }



    }

    
}
