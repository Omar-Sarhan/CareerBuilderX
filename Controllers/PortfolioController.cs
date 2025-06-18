using CareerBuilderX.AI.Interfaces;
using CareerBuilderX.AI.Services;
using CareerBuilderX.DTOs;
using CareerBuilderX.Models;
using CareerBuilderX.Repositories.Interfaces;
using CareerBuilderX.Repositories.Repo;
using Humanizer.Bytes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using System.Data;
using System.Security.Claims;
using static CareerBuilderX.DTOs.EditPortfolioDTO;

namespace CareerBuilderX.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private IPortfolioAiService _portfolioAiService { get; set; }

        private IServiceRepository _serviceRepository { get; set; }

        public PortfolioController(IPortfolioRepository portfolioRepository , IPortfolioAiService portfolioAiService , IServiceRepository serviceRepository)
        {
            _portfolioRepository = portfolioRepository;

            _portfolioAiService = portfolioAiService;

            _serviceRepository = serviceRepository;
        }

        private string GetUserLoginId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [Authorize(Roles = "EndUser")]
        public IActionResult ListOfPortfolios()
        {
            var portfolios = _portfolioRepository.GetAllPortfoliosByUser(GetUserLoginId());
            ViewBag.Portfolios = portfolios;
            return View();
        }

        [Authorize(Roles = "EndUser")]
        public IActionResult CreatePortfolio()
        {
            ViewBag.Services = _serviceRepository.GetAllServices();
            return View();
        }
        [Authorize(Roles = "EndUser")]
        [HttpPost]
        public async Task<IActionResult> CreatePortfolio(PortfolioDTO portfolioDTO)
        {
            ViewBag.Services = _serviceRepository.GetAllServices();

            byte[]? byteFile = null;

            if (portfolioDTO.ProfileImage?.Length > 0)
            {
                using var br = new BinaryReader(portfolioDTO.ProfileImage.OpenReadStream());
                byteFile = br.ReadBytes((int)portfolioDTO.ProfileImage.Length);
            }

            if (!ModelState.IsValid)
                return View(portfolioDTO);

            try
            {

                var projectsText = string.Join("\n", portfolioDTO.Projects.Select(p =>
                    $"- {p.Title}: {p.ProjectDescription}"));

                var combinedInput = $@"
                    About Me:
                    {portfolioDTO.About}

                    Projects:
                    {projectsText}

                    Skills: 
                    {portfolioDTO.SkillsInfo}
                    ";

                var aiPortfolio = await _portfolioAiService.ParseUserInputAsync(combinedInput);

                // -------------------------------
                // 3. Validate AI output
                var errors = new List<string>();
                if (string.IsNullOrWhiteSpace(aiPortfolio.FName) || aiPortfolio.FName == "not provided")
                    errors.Add("First name is required.");
                if (string.IsNullOrWhiteSpace(aiPortfolio.Lname) || aiPortfolio.Lname == "not provided")
                    errors.Add("Last name is required.");
                if (string.IsNullOrWhiteSpace(aiPortfolio.Title) || aiPortfolio.Title == "not provided")
                    errors.Add("Job title is required.");
                if (string.IsNullOrWhiteSpace(aiPortfolio.Summary) || aiPortfolio.Summary == "not provided")
                    errors.Add("Professional summary is required.");
                if (portfolioDTO.Projects != null && portfolioDTO.Projects.Any())
                {
                    for (int i = 0; i < aiPortfolio.Projects.Count; i++)
                    {
                        var proj = aiPortfolio.Projects[i];
                        if (string.IsNullOrWhiteSpace(proj.Title) || proj.Title == "not provided")
                            errors.Add($"Project #{i + 1}: title is required.");
                        if (string.IsNullOrWhiteSpace(proj.ProjectDescription) || proj.ProjectDescription == "not provided")
                            errors.Add($"Project #{i + 1}: description is required.");
                    }
                }
                if (errors.Any())
                {
                    ViewBag.Errors = errors;
                    // Re-populate Services and DTO so the form can re-render
                    ViewBag.Services = _serviceRepository.GetAllServices();
                    return View(portfolioDTO);
                }


                // 🏗️ 2. أنشئ كائن Portfolio مع البيانات المستخرجة
                var newPortfolio = new Portfolio
                {
                    FName = aiPortfolio.FName,
                    Lname = aiPortfolio.Lname,
                    Title = aiPortfolio.Title,
                    Summary = aiPortfolio.Summary,
                    Email = portfolioDTO.Email,
                    Address = portfolioDTO.Address,
                    LinkedInLink = portfolioDTO.LinkedInLink,
                    GitHubLink = portfolioDTO.GitHubLink,
                    FaceBookLink = portfolioDTO.FaceBookLink,
                    TwitterLink = portfolioDTO.TwitterLink,
                    PhoneNumber = portfolioDTO.PhoneNumber,
                    About = portfolioDTO.About,
                    CreatedDate = DateTime.Now,
                    CreatedModifiedDate = DateTime.Now,
                    EndUserId = GetUserLoginId(),
                    profileImgName = portfolioDTO.ProfileImage.FileName,
                    profileImgType = portfolioDTO.ProfileImage.ContentType,
                    ProfileImg = byteFile,
                    Projects = new List<Project>(),
                    Skills = new List<Skill>(),
                    servicePortfolios = new List<ServicePortfolio>()
                };

                if (aiPortfolio.Skills != null)
                {
                    foreach (var s in aiPortfolio.Skills)
                    {
                        newPortfolio.Skills.Add(new Skill
                        {
                            SkillName = s.SkillName,
                            Portfolio = newPortfolio
                        });
                    }
                }




                // 🧱 3. أضف المشاريع (اللي رجعها الـ AI)
                for (int i = 0; i < aiPortfolio.Projects.Count; i++)
                {
                    var aiProject = aiPortfolio.Projects[i];
                    var userProject = portfolioDTO.Projects.ElementAtOrDefault(i);

                    byte[]? projectImg = null;
                    string? imgName = null;
                    string? imgType = null;

                    if (userProject?.ProjectImage != null && userProject.ProjectImage.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        await userProject.ProjectImage.CopyToAsync(ms);
                        projectImg = ms.ToArray();
                        imgName = userProject.ProjectImage.FileName;
                        imgType = userProject.ProjectImage.ContentType;
                    }

                    newPortfolio.Projects.Add(new Project
                    {
                        Title = aiProject.Title,
                        ProjectDescription = aiProject.ProjectDescription,
                        ProjectImg = projectImg,
                        projectImgName = imgName,
                        prjectImgType = imgType,
                        Portfolio = newPortfolio,

                        ProjectLink = userProject?.ProjectLink,
                        StartDate = userProject?.StartDate,
                        EndDate = userProject?.EndDate,
                    });
                }

                


                // 🔗 4. اربط الخدمات المختارة
                foreach (var serviceId in portfolioDTO.ServiceIds)
                {
                    newPortfolio.servicePortfolios.Add(new ServicePortfolio
                    {
                        Portfolio = newPortfolio,
                        ServiceId = serviceId
                    });
                }

                // 💾 5. خزّن في الداتا بيس
                _portfolioRepository.CreatePortfolio(newPortfolio);
                return RedirectToAction("ListOfPortfolios");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"❌ An error occurred: {ex.Message}");
                return View(portfolioDTO);
            }
        }

        [Authorize(Roles = "EndUser")]
        public IActionResult EditPortfolio(int id)
        {
            var existing = _portfolioRepository.GetPortfolioById(id);
            if (existing == null) return NotFound();

            var dto = new EditPortfolioDTO
            {
                Id = existing.PortfolioId,
                FName = existing.FName,
                Lname = existing.Lname,
                Title = existing.Title,
                Summary = existing.Summary,
                Email = existing.Email,
                PhoneNumber = existing.PhoneNumber,
                Address = existing.Address,
                LinkedInLink = existing.LinkedInLink,
                GitHubLink = existing.GitHubLink,
                FaceBookLink = existing.FaceBookLink,
                TwitterLink = existing.TwitterLink,
                ServiceIds = existing.servicePortfolios.Select(sp => sp.ServiceId).ToList(),
                ExistingProfileImg = existing.ProfileImg,
                ExistingProfileImgType = existing.profileImgType,
                Skills = existing.Skills.Select(s => new SkillsEditDTO
                {
                    Id = s.SkillId,
                    skillName = s.SkillName
                }).ToList(),
                Projects = existing.Projects.Select(p => new ProjectEditDTO
                {
                    Id = p.ProjectId,            
                    Title = p.Title,
                    ProjectDescription = p.ProjectDescription,
                    ExistingProjectImg = p.ProjectImg,
                    ExistingProjectImgType = p.prjectImgType,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    ProjectLink = p.ProjectLink
                }).ToList()
            };
            ViewBag.Services = _serviceRepository.GetAllServices();
            return View(dto);
        }

        [Authorize(Roles = "EndUser")]
        [HttpPost]
        public async Task<IActionResult> EditPortfolio(EditPortfolioDTO dto)
        {
            // 1. Validate model
            if (!ModelState.IsValid)
            {
                // re‐load services
                ViewBag.Services = _serviceRepository.GetAllServices();

                // re‐load existing images from DB so your view can show them
                var existing1 = _portfolioRepository.GetPortfolioById(dto.Id)!;
                dto.Skills = existing1.Skills
                   .Select(s => new SkillsEditDTO
                   {
                       Id = s.SkillId,
                       skillName = s.SkillName
                   }).ToList();

                dto.ExistingProfileImg = existing1.ProfileImg;
                dto.ExistingProfileImgType = existing1.profileImgType;

                // map each project’s existing image back onto the dto
                foreach (var pDto in dto.Projects)
                {
                    var projInDb = existing1.Projects
                                          .FirstOrDefault(p => p.ProjectId == pDto.Id);
                    if (projInDb != null)
                    {
                        pDto.ExistingProjectImg = projInDb.ProjectImg;
                        pDto.ExistingProjectImgType = projInDb.prjectImgType;
                    }
                }

                return View(dto);
            }

            // 2. Fetch the existing portfolio
            var existing = _portfolioRepository.GetPortfolioById(dto.Id);
            if (existing == null) return NotFound();

            // 3. Update text fields directly (no AI)
            existing.FName = dto.FName;
            existing.Lname = dto.Lname;
            existing.Title = dto.Title;
            existing.Summary = dto.Summary;
            existing.Email = dto.Email;
            existing.PhoneNumber = dto.PhoneNumber;
            existing.Address = dto.Address;
            existing.LinkedInLink = dto.LinkedInLink;
            existing.GitHubLink = dto.GitHubLink;
            existing.FaceBookLink = dto.FaceBookLink;
            existing.TwitterLink = dto.TwitterLink;
            existing.CreatedModifiedDate = DateTime.Now;

            // 4. Handle profile image replacement
            if (dto.ProfileImage != null && dto.ProfileImage.Length > 0)
            {
                using var br = new BinaryReader(dto.ProfileImage.OpenReadStream());
                existing.ProfileImg = br.ReadBytes((int)dto.ProfileImage.Length);
                existing.profileImgType = dto.ProfileImage.ContentType;
            }
            // otherwise leave existing.ProfileImg as-is

            // 5. Replace selected services
            existing.servicePortfolios = dto.ServiceIds
                .Select(sid => new ServicePortfolio
                {
                    PortfolioId = dto.Id,
                    ServiceId = sid
                }).ToList();
            if (dto.Projects != null)
            {
                var projectIdsInDto = dto.Projects
                    .Where(p => p.Id.HasValue && p.Id > 0)
                    .Select(p => p.Id.Value)
                    .ToHashSet();

                foreach (var p in existing.Projects.ToList())
                    if (!projectIdsInDto.Contains(p.ProjectId))
                        _portfolioRepository.DeleteProject(p.ProjectId);

                foreach (var pDto in dto.Projects)
                {
                    if (pDto.Id.HasValue && pDto.Id > 0)
                    {
                        var proj = existing.Projects.FirstOrDefault(p => p.ProjectId == pDto.Id.Value);
                        if (proj != null)
                        {
                            proj.Title = pDto.Title;
                            proj.ProjectDescription = pDto.ProjectDescription;
                            proj.StartDate = pDto.StartDate;
                            proj.EndDate = pDto.EndDate;
                            proj.ProjectLink = pDto.ProjectLink;

                            if (pDto.ProjectImage?.Length > 0)
                            {
                                using var ms = new MemoryStream();
                                await pDto.ProjectImage.CopyToAsync(ms);
                                proj.ProjectImg = ms.ToArray();
                                proj.prjectImgType = pDto.ProjectImage.ContentType;
                            }
                        }
                    }
                    else
                    {
                        byte[]? img = null;
                        string? type = null;

                        if (pDto.ProjectImage?.Length > 0)
                        {
                            using var ms = new MemoryStream();
                            await pDto.ProjectImage.CopyToAsync(ms);
                            img = ms.ToArray();
                            type = pDto.ProjectImage.ContentType;
                        }

                        existing.Projects.Add(new Project
                        {
                            Title = pDto.Title,
                            ProjectDescription = pDto.ProjectDescription,
                            ProjectImg = img,
                            prjectImgType = type,
                            Portfolio = existing
                        });
                    }
                }
            }






            if (dto.Skills != null)
            {
                var skillIdsInDto = dto.Skills
                    .Where(s => s.Id.HasValue && s.Id > 0)
                    .Select(s => s.Id.Value)
                    .ToHashSet();

                foreach (var s in existing.Skills.ToList())
                {
                    if (!skillIdsInDto.Contains(s.SkillId))
                        _portfolioRepository.DeleteSkill(s.SkillId);
                }

                foreach (var sDto in dto.Skills)
                {
                    if (sDto.Id.HasValue && sDto.Id > 0)
                    {
                        var skill = existing.Skills.FirstOrDefault(s => s.SkillId == sDto.Id.Value);
                        if (skill != null)
                        {
                            skill.SkillName = sDto.skillName;
                        }
                    }
                    else
                    {
                        existing.Skills.Add(new Skill
                        {
                            SkillName = sDto.skillName,
                            Portfolio = existing
                        });
                    }
                }
            }




            // 7. Persist changes
            _portfolioRepository.UpdatePortfolio(existing);

            return RedirectToAction("ListOfPortfolios");
        }

        [Authorize(Roles = "EndUser")]
        public IActionResult PortfolioPage(int id)
        {
            Portfolio portfolio = _portfolioRepository.GetPortfolioById(id);
            return View(portfolio);
        }
        public IActionResult Landing()
        {
            return View();
        }

        public FileStreamResult GetFile(int id)
        {
            var file = _portfolioRepository.GetPortfolioById(id);
            Stream stream = new MemoryStream(file.ProfileImg);
            return new FileStreamResult(stream, file.profileImgType);
        }
        [Authorize(Roles = "EndUser")]
        public IActionResult DeletePortfolio(int portfolioId)
        {
            var portfolio = _portfolioRepository.GetPortfolioById(portfolioId);
            if (portfolio != null)
            {
                _portfolioRepository.DeletePortfolio(portfolio.PortfolioId);
            }

            TempData["Deleted"] = "true"; // نحدد انه تم الحذف
            return RedirectToAction("ListOfPortfolios");
        }
    }
}
