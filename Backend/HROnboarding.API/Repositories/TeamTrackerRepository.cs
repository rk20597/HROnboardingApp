using OfficeOpenXml;
using HROnboarding.API.Models;

namespace HROnboarding.API.Repositories
{
    public class TeamTrackerRepository
    {
        private readonly string _filePath;
        private static readonly SemaphoreSlim _lock
            = new SemaphoreSlim(1, 1);

        public TeamTrackerRepository(string filePath)
        {
            _filePath = filePath;
        }

        // ==================
        // TEAM MEMBERS
        // ==================

        public async Task<List<TeamMember>>
            GetAllMembers()
        {
            await _lock.WaitAsync();
            try
            {
                var members = new List<TeamMember>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim() == "TeamMember")
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return members;

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    var name = sheet.Cells[row, 2]
                        .Value?.ToString();
                    if (string.IsNullOrEmpty(name))
                        continue;

                    members.Add(new TeamMember
                    {
                        SrNo = row - 1,
                        Name = name,
                        Email = sheet.Cells[row, 3]
                            .Value?.ToString(),
                        Location = sheet.Cells[row, 4]
                            .Value?.ToString(),
                        Status = sheet.Cells[row, 5]
                            .Value?.ToString(),
                        Expansion = sheet.Cells[row, 6]
                            .Value?.ToString(),
                        City = sheet.Cells[row, 7]
                            .Value?.ToString(),
                        LevelOriginal = sheet
                            .Cells[row, 8]
                            .Value?.ToString(),
                        Level = sheet.Cells[row, 9]
                            .Value?.ToString(),
                        ClientLevel = sheet
                            .Cells[row, 10]
                            .Value?.ToString(),
                        OnboardingDate = sheet
                            .Cells[row, 11]
                            .Value?.ToString(),
                        Joined = sheet.Cells[row, 12]
                            .Value?.ToString(),
                        JobFamily = sheet
                            .Cells[row, 13]
                            .Value?.ToString(),
                        GDLeader1 = sheet
                            .Cells[row, 14]
                            .Value?.ToString(),
                        GDLeader2 = sheet
                            .Cells[row, 15]
                            .Value?.ToString(),
                        GDLeader3 = sheet
                            .Cells[row, 16]
                            .Value?.ToString(),
                        Project = sheet.Cells[row, 17]
                            .Value?.ToString(),
                        ClientKTDelivery = sheet
                            .Cells[row, 18]
                            .Value?.ToString(),
                        ProjectDeliveryStarted = sheet
                            .Cells[row, 19]
                            .Value?.ToString(),
                        ToolAccessCompleted = sheet
                            .Cells[row, 20]
                            .Value?.ToString(),
                        Datadog = sheet.Cells[row, 21]
                            .Value?.ToString(),
                        AKSTrained = sheet
                            .Cells[row, 22]
                            .Value?.ToString(),
                        ROVO = sheet.Cells[row, 23]
                            .Value?.ToString(),
                        AIFluency = sheet
                            .Cells[row, 24]
                            .Value?.ToString(),
                        Claude101 = sheet
                            .Cells[row, 25]
                            .Value?.ToString(),
                        OtherAICertification = sheet
                            .Cells[row, 26]
                            .Value?.ToString(),
                        CoPilotTrained = sheet
                            .Cells[row, 27]
                            .Value?.ToString(),
                        ClientComplianceTrainings = sheet
                            .Cells[row, 28]
                            .Value?.ToString(),
                        SciFormaAccess = sheet
                            .Cells[row, 29]
                            .Value?.ToString(),
                        OffboardingDate = sheet
                            .Cells[row, 30]
                            .Value?.ToString(),
                        Comments = sheet
                            .Cells[row, 31]
                            .Value?.ToString(),
                        MobileNumber = sheet
                            .Cells[row, 32]
                            .Value?.ToString(),
                        Replacement = sheet
                            .Cells[row, 33]
                            .Value?.ToString()
                    });
                }
                return members;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<List<TeamMember>>
            GetActiveMembers()
        {
            var all = await GetAllMembers();
            return all.Where(m =>
                m.Status?.ToLower() == "active")
                .ToList();
        }

        public async Task<List<TeamMember>>
            GetInactiveMembers()
        {
            var all = await GetAllMembers();
            return all.Where(m =>
                m.Status?.ToLower() == "inactive")
                .ToList();
        }

        // ==================
        // ONBOARDING STEPS
        // ==================

        public async Task<List<OnboardingStep>>
            GetOnboardingSteps()
        {
            await _lock.WaitAsync();
            try
            {
                var steps = new List<OnboardingStep>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim() ==
                        "OnboardingSteps")
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return steps;

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    var stepName = sheet.Cells[row, 2]
                        .Value?.ToString();
                    if (string.IsNullOrEmpty(stepName))
                        continue;
                    steps.Add(new OnboardingStep
                    {
                        StepID = Convert.ToInt32(
                            sheet.Cells[row, 1]
                            .Value ?? 0),
                        StepName = stepName,
                        StepOrder = Convert.ToInt32(
                            sheet.Cells[row, 3]
                            .Value ?? 0),
                        Description = sheet
                            .Cells[row, 4]
                            .Value?.ToString()
                    });
                }
                return steps;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task AddOnboardingStep(
            OnboardingStep step)
        {
            await _lock.WaitAsync();
            try
            {
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim() ==
                        "OnboardingSteps")
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return;

                int newRow = sheet.Dimension
                    .End.Row + 1;
                sheet.Cells[newRow, 1].Value =
                    newRow - 1;
                sheet.Cells[newRow, 2].Value =
                    step.StepName;
                sheet.Cells[newRow, 3].Value =
                    step.StepOrder;
                sheet.Cells[newRow, 4].Value =
                    step.Description;

                await package.SaveAsync();
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task UpdateOnboardingStep(
            OnboardingStep step)
        {
            await _lock.WaitAsync();
            try
            {
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim() ==
                        "OnboardingSteps")
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return;

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    var id = Convert.ToInt32(
                        sheet.Cells[row, 1].Value ?? 0);
                    if (id == step.StepID)
                    {
                        sheet.Cells[row, 2].Value =
                            step.StepName;
                        sheet.Cells[row, 3].Value =
                            step.StepOrder;
                        sheet.Cells[row, 4].Value =
                            step.Description;
                        break;
                    }
                }
                await package.SaveAsync();
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task DeleteOnboardingStep(
            int stepId)
        {
            await _lock.WaitAsync();
            try
            {
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim() ==
                        "OnboardingSteps")
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return;

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    var id = Convert.ToInt32(
                        sheet.Cells[row, 1].Value ?? 0);
                    if (id == stepId)
                    {
                        sheet.DeleteRow(row);
                        break;
                    }
                }
                await package.SaveAsync();
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<List<OnboardingProgress>>
    GetOnboardingProgress()
        {
            await _lock.WaitAsync();
            try
            {
                var progress =
                    new List<OnboardingProgress>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim() ==
                        "OnboardingProcess")
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return progress;

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    progress.Add(new OnboardingProgress
                    {
                        ProgressID = Convert.ToInt32(
                            sheet.Cells[row, 1]
                            .Value ?? 0),
                        CandidateID = Convert.ToInt32(
                            sheet.Cells[row, 2]
                            .Value ?? 0),
                        StepID = Convert.ToInt32(
                            sheet.Cells[row, 3]
                            .Value ?? 0),
                        CompletedDate = sheet
                            .Cells[row, 4]
                            .Value?.ToString(),
                        Status = sheet.Cells[row, 5]
                            .Value?.ToString()
                    });
                }
                return progress;
            }
            finally
            {
                _lock.Release();
            }
        }


        // ==================
        // TRAINING LINKS
        // ==================

        public async Task<List<TrainingCourse>>
            GetTrainingCourses(string sheetName)
        {
            await _lock.WaitAsync();
            try
            {
                var courses = new List<TrainingCourse>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim() == sheetName)
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return courses;

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    var title = sheet.Cells[row, 2]
                        .Value?.ToString();
                    if (string.IsNullOrEmpty(title))
                        continue;
                    courses.Add(new TrainingCourse
                    {
                        Platform = sheet.Cells[row, 1]
                            .Value?.ToString(),
                        Title = title,
                        Link = sheet.Cells[row, 3]
                            .Value?.ToString(),
                        EstTime = sheet.Cells[row, 4]
                            .Value?.ToString(),
                        Remark = sheet.Cells[row, 5]
                            .Value?.ToString(),
                        Domain = sheetName
                    });
                }
                return courses;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<List<TrainingLink>>
            GetTrainingLinks(string sheetName)
        {
            await _lock.WaitAsync();
            try
            {
                var links = new List<TrainingLink>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim() == sheetName)
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return links;

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    var topic = sheet.Cells[row, 1]
                        .Value?.ToString();
                    if (string.IsNullOrEmpty(topic))
                        continue;
                    links.Add(new TrainingLink
                    {
                        Topic = topic,
                        Link = sheet.Cells[row, 2]
                            .Value?.ToString(),
                        Domain = sheetName
                    });
                }
                return links;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<List<MandatoryTrainings>>
            GetMandatoryTrainings()
        {
            await _lock.WaitAsync();
            try
            {
                var trainings =
                    new List<MandatoryTrainings>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim() == "Madatory Trainings")
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return trainings;

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    var jobFamily = sheet.Cells[row, 1]
                        .Value?.ToString();
                    if (string.IsNullOrEmpty(jobFamily))
                        continue;
                    trainings.Add(new MandatoryTrainings
                    {
                        JobFamily = jobFamily,
                        MandatoryTraining = sheet
                            .Cells[row, 2]
                            .Value?.ToString(),
                        POC = sheet.Cells[row, 3]
                            .Value?.ToString()
                    });
                }
                return trainings;
            }
            finally
            {
                _lock.Release();
            }
        }

        // ==================
        // PROJECTS
        // ==================

        public async Task<List<Projects>> GetProjects()
        {
            await _lock.WaitAsync();
            try
            {
                var projects = new List<Projects>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim()
                        .Contains("Projects"))
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return projects;

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    var name = sheet.Cells[row, 2]
                        .Value?.ToString();
                    if (string.IsNullOrEmpty(name))
                        continue;
                    projects.Add(new Projects
                    {
                        Sno = sheet.Cells[row, 1]
                            .Value?.ToString(),
                        ProjectName = name,
                        Status = sheet.Cells[row, 3]
                            .Value?.ToString(),
                        TeamCount = sheet.Cells[row, 4]
                            .Value?.ToString(),
                        DeloitteTeamLead = sheet
                            .Cells[row, 5]
                            .Value?.ToString(),
                        HasUSATeam = sheet.Cells[row, 6]
                            .Value?.ToString(),
                        HasADMXTeam = sheet
                            .Cells[row, 7]
                            .Value?.ToString(),
                        ScrumMasterOffshore = sheet
                            .Cells[row, 8]
                            .Value?.ToString(),
                        ScrumMasterOnshore = sheet
                            .Cells[row, 9]
                            .Value?.ToString(),
                        ScrumMasterRelease = sheet
                            .Cells[row, 10]
                            .Value?.ToString(),
                        MeetingCadence = sheet
                            .Cells[row, 11]
                            .Value?.ToString()
                    });
                }
                return projects;
            }
            finally
            {
                _lock.Release();
            }
        }

        // ==================
        // GD LEAD
        // ==================

        public async Task<List<GDLead>> GetGDLeadData()
        {
            await _lock.WaitAsync();
            try
            {
                var leads = new List<GDLead>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim() == "GDLead")
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return leads;

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    var skillset = sheet.Cells[row, 1]
                        .Value?.ToString();
                    if (string.IsNullOrEmpty(skillset))
                        continue;
                    leads.Add(new GDLead
                    {
                        SkillSet = skillset,
                        GDLeadName = sheet
                            .Cells[row, 2]
                            .Value?.ToString(),
                        TotalResources = sheet
                            .Cells[row, 3]
                            .Value?.ToString(),
                        AdditionalTools = sheet
                            .Cells[row, 4]
                            .Value?.ToString(),
                        KeyHighlights = sheet
                            .Cells[row, 5]
                            .Value?.ToString()
                    });
                }
                return leads;
            }
            finally
            {
                _lock.Release();
            }
        }

        // ==================
        // RAW SHEET DATA
        // For pivot tables
        // ==================

        public async Task<List<Dictionary
            <string, string>>>
            GetRawSheetData(string sheetName)
        {
            await _lock.WaitAsync();
            try
            {
                var data = new List<Dictionary
                    <string, string>>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                ExcelWorksheet? sheet = null;
                foreach (var ws in package.Workbook
                    .Worksheets)
                {
                    if (ws.Name.Trim() == sheetName)
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null) return data;

                var headers = new List<string>();
                for (int col = 1; col <= sheet
                    .Dimension.End.Column; col++)
                {
                    headers.Add(sheet.Cells[1, col]
                        .Value?.ToString() ??
                        $"Col{col}");
                }

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    var rowData = new Dictionary
                        <string, string>();
                    for (int col = 1; col <=
                        headers.Count; col++)
                    {
                        rowData[headers[col - 1]] =
                            sheet.Cells[row, col]
                            .Value?.ToString() ?? "";
                    }
                    data.Add(rowData);
                }
                return data;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
