using CareerBuilderX.AI.Interfaces;
using CareerBuilderX.DTOs;
using CareerBuilderX.Models;
using CareerBuilderX.Repositories.Interfaces;
using CareerBuilderX.Repository.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Services;
using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using CareerBuilderX.Repositories.Repo;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using QuestPDF.Fluent;

namespace CareerBuilderX.Controllers
{
    public class ResumeController : Controller
    {



        private IResumeRepository resumeRepository { get; set; }
        private IResumeAiService ResumeAiService { get; set; }
        public string GetUserLoginId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public ResumeController(IResumeRepository resumeRepository, IResumeAiService resumeAiService )
        {
            this.resumeRepository = resumeRepository;
            this.ResumeAiService = resumeAiService;
        }

        [Authorize(Roles = "EndUser")]
        public IActionResult ListOfResumes()
        {
            var userid = GetUserLoginId();
            ViewBag.Resume = resumeRepository.GetAllResumesByUser(userid);
            return View();
        }
        public IActionResult Details(int id)
        {
            var resume = resumeRepository.GetResumeById(id);
            return View(resume);
        }

        [Authorize(Roles = "EndUser")]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var resume = resumeRepository.GetResumeById(id);
            if (resume == null)
                return NotFound();

            // توليد ملف PDF في الذاكرة
            var document = new ResumeDocument(resume);
            var pdfBytes = document.GeneratePdf();

            // إعادة الملف للمستخدم
            return File(pdfBytes, "application/pdf", $"{resume.FName}{resume.Lname}_Resume.pdf");
        }




        [Authorize(Roles = "EndUser")]
        public IActionResult CreateResume()
        {
            return View();
        }
        [Authorize(Roles = "EndUser")]
        [HttpPost]
        public async Task<IActionResult> CreateResume(ResumeDTO resumeDTO)
        {
            if (!ModelState.IsValid)
                return View(resumeDTO);

            try
            {
                var combinedInput = $@"
                About: {resumeDTO.AboutYou},
                Career Info: {resumeDTO.CareerInfo},
                Education Info: {resumeDTO.EducationInfo},
                Language Info: {resumeDTO.LanguageInfo},
                Certification Info: {resumeDTO.CertificationInfo},
                Skills Info: {resumeDTO.SkillsInfo}";



                var resume = await ResumeAiService.ParseUserInputAsync(combinedInput);



                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(resume.FName) || resume.FName == "not provided")
                    errors.Add("First name is required.");

                if (string.IsNullOrWhiteSpace(resume.Lname) || resume.Lname == "not provided")
                    errors.Add("Last name is required.");

                foreach (var exp in resume.Experiences)
                {
                    if (string.IsNullOrWhiteSpace(exp.Title) || exp.Title == "not provided")
                        errors.Add("Experience title is required.");

                    if (string.IsNullOrWhiteSpace(exp.CompanyName) || exp.CompanyName == "not provided")
                        errors.Add("Company name is required in experience.");
                }

                foreach (var edu in resume.Educations)
                {
                    if (string.IsNullOrWhiteSpace(edu.CollegeName) || edu.CollegeName == "not provided")
                        errors.Add("College name is required.");

                    if (string.IsNullOrWhiteSpace(edu.Major) || edu.Major == "not provided")
                        errors.Add("Major is required.");

                    if (string.IsNullOrWhiteSpace(edu.Degree) || edu.Degree == "not provided")
                        errors.Add("Degree is required.");
                }

                if (errors.Count > 0)
                {
                    ViewBag.Errors = errors;
                    return View(resumeDTO); // يرجع لنفس صفحة Create ويعرض الأخطاء
                }

                resume.Email = resumeDTO.Email;
                resume.Address = resumeDTO.Address;
                resume.PhoneNumber = resumeDTO.PhoneNumber;
                resume.EndUserId = GetUserLoginId();
                resume.CareerInfo = resumeDTO.CareerInfo;
                resume.CertificationInfo = resumeDTO.CertificationInfo;
                resume.SkillsInfo = resumeDTO.SkillsInfo;
                resume.EducationInfo = resumeDTO.EducationInfo;
                resume.LanguageInfo = resumeDTO.LanguageInfo;
                resume.About = resumeDTO.AboutYou;
                resume.GitHubLink = resumeDTO.GitHubLink;
                resume.LinkedInLink = resumeDTO.LinkedInLink;
                resumeRepository.CreateResume(resume);
                TempData["SuccessMessage"] = "Your resume has been created successfully!";
                return RedirectToAction("ListOfResumes");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "❌ AI Error: " + ex.Message);
                return View(resumeDTO);
            }
        }


        [Authorize(Roles = "EndUser")]
        public IActionResult EditResume(int id)
        {
            var resume = resumeRepository.GetResumeById(id);
            if (resume == null || resume.EndUserId != GetUserLoginId())
                return NotFound();

            // تحويل Resume إلى ResumeDTO لتعبئة النصوص
            var dto = new ResumeDTO
            {
                AboutYou = resume.About,
                CareerInfo = resume.CareerInfo,
                EducationInfo = resume.EducationInfo,
                LanguageInfo = resume.LanguageInfo,
                CertificationInfo = resume.CertificationInfo,
                SkillsInfo = resume.SkillsInfo,
                Email = resume.Email,
                PhoneNumber = resume.PhoneNumber,
                Address = resume.Address,
                LinkedInLink = resume.LinkedInLink,
                GitHubLink = resume.GitHubLink
            };
            return View(dto);
        }

        [Authorize(Roles = "EndUser")]
        [HttpPost]
        public async Task<IActionResult> EditResume(int id, ResumeDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var existingResume = resumeRepository.GetResumeById(id);
            if (existingResume == null || existingResume.EndUserId != GetUserLoginId())
                return NotFound();

            if(dto.LanguageInfo != existingResume.LanguageInfo || dto.SkillsInfo != existingResume.SkillsInfo 
                || dto.EducationInfo != existingResume.EducationInfo || dto.CertificationInfo != existingResume.CertificationInfo
                || dto.CareerInfo !=  existingResume.CareerInfo 
                || dto.AboutYou != existingResume.About)
            {
                // استدعاء AI لتحليل النصوص وإعادة بناء Resume object
                var updatedResume = await ResumeAiService.ParseUserInputAsync($@"
        About: {dto.AboutYou},
        Career Info: {dto.CareerInfo},
        Education Info: {dto.EducationInfo},
        Language Info: {dto.LanguageInfo},
        Certification Info: {dto.CertificationInfo},
        Skills Info: {dto.SkillsInfo}");

                existingResume.FName = updatedResume.FName;
                existingResume.Lname = updatedResume.Lname;
                existingResume.Title = updatedResume.Title;
                existingResume.Experiences = updatedResume.Experiences;
                existingResume.Educations = updatedResume.Educations;
                existingResume.Skills = updatedResume.Skills;
                existingResume.Certifications = updatedResume.Certifications;
                existingResume.Languages = updatedResume.Languages;
            }
            

            // نسخ الحقول الأساسية من DTO و AI object إلى الموجود
            existingResume.About = dto.AboutYou;
            existingResume.CareerInfo = dto.CareerInfo;
            existingResume.EducationInfo = dto.EducationInfo;
            existingResume.LanguageInfo = dto.LanguageInfo;
            existingResume.CertificationInfo = dto.CertificationInfo;
            existingResume.SkillsInfo = dto.SkillsInfo;
            existingResume.Email = dto.Email;
            existingResume.PhoneNumber = dto.PhoneNumber;
            existingResume.Address = dto.Address;
            existingResume.LinkedInLink = dto.LinkedInLink;
            existingResume.GitHubLink = dto.GitHubLink;

            existingResume.CreatedModifiedDate = DateTime.Now;

            resumeRepository.UpdateResume(existingResume);
            TempData["SuccessUpdateMessage"] = "Your resume has been updated successfully!";
            return RedirectToAction("ListOfResumes");
        }

        public IActionResult Landing()
        {
            return View();
        }

        [Authorize(Roles = "EndUser")]
        public IActionResult DeleteResume(int ResumeId)
        {
            var resume = resumeRepository.GetResumeById(ResumeId);
            if (resume != null)
            {
                resumeRepository.DeleteResume(resume.ResumeId);
            }

            TempData["Deleted"] = "true"; // نحدد انه تم الحذف
            return RedirectToAction("ListOfResumes");
        }


    }
}
