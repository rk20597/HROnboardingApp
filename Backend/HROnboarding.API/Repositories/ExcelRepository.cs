using OfficeOpenXml;
using HROnboarding.API.Models;

namespace HROnboarding.API.Repositories
{
    public class ExcelRepository
    {
        private readonly string _filePath;
        private static readonly SemaphoreSlim _lock
            = new SemaphoreSlim(1, 1);

        public ExcelRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<Candidate>>
            GetAllCandidates()
        {
            await _lock.WaitAsync();
            try
            {
                var candidates = new List<Candidate>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));
                var sheet = package.Workbook
                    .Worksheets[1];
                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    candidates.Add(new Candidate
                    {
                        CandidateID = Convert.ToInt32(
                            sheet.Cells[row, 1].Value),
                        Name = sheet.Cells[row, 2]
                            .Value?.ToString(),
                        Email = sheet.Cells[row, 3]
                            .Value?.ToString(),
                        Role = sheet.Cells[row, 4]
                            .Value?.ToString(),
                        status = sheet.Cells[row, 5]
                            .Value?.ToString(),
                        JoinDate = sheet.Cells[row, 6]
                            .Value?.ToString(),
                        Team = sheet.Cells[row, 7]
                            .Value?.ToString()
                    });
                }
                return candidates;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<List<Candidate>>
            GetActiveCandidates()
        {
            var all = await GetAllCandidates();
            return all.Where(c =>
                c.status == "Active").ToList();
        }

        public async Task<List<Candidate>>
            GetInactiveCandidates()
        {
            var all = await GetAllCandidates();
            return all.Where(c =>
                c.status == "Inactive").ToList();
        }

        public async Task<User?> GetUserByUsername(
    string username)
        {
            await _lock.WaitAsync();
            try
            {
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));

                // Print all sheet names for debug
                Console.WriteLine("Sheet count: " +
                    package.Workbook.Worksheets.Count);

                foreach (var ws in
                    package.Workbook.Worksheets)
                {
                    Console.WriteLine("Found sheet: [" +
                        ws.Name + "]");
                }

                // Try getting sheet by name
                ExcelWorksheet? sheet = null;
                foreach (var ws in
                    package.Workbook.Worksheets)
                {
                    if (ws.Name.Trim() == "Users")
                    {
                        sheet = ws;
                        break;
                    }
                }

                if (sheet == null)
                {
                    Console.WriteLine("Users sheet NOT found");
                    return null;
                }

                Console.WriteLine("Users sheet found!");

                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    var uname = sheet.Cells[row, 2]
                        .Value?.ToString();
                    Console.WriteLine("Checking user: " +
                        uname);
                    if (uname?.Trim() == username.Trim())
                    {
                        return new User
                        {
                            UserID = Convert.ToInt32(
                                sheet.Cells[row, 1].Value),
                            UserName = uname,
                            PasswordHash = sheet
                                .Cells[row, 3]
                                .Value?.ToString(),
                            Role = sheet.Cells[row, 4]
                                .Value?.ToString(),
                            IsActive = sheet
                                .Cells[row, 5]
                                .Value?.ToString()
                                ?.ToLower() == "true"
                        };
                    }
                }
                return null;
            }
            finally
            {
                _lock.Release();
            }
        }


        public async Task<List<Training>>
            GetAllTraining()
        {
            await _lock.WaitAsync();
            try
            {
                var trainings = new List<Training>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));
                var sheet = package.Workbook
                    .Worksheets[3];
                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    trainings.Add(new Training
                    {
                        CandidateID = Convert.ToInt32(
                            sheet.Cells[row, 1].Value),
                        Datadog = sheet.Cells[row, 2]
                            .Value?.ToString(),
                        AKSTrained = sheet.Cells[row, 3]
                            .Value?.ToString(),
                        ROVO = sheet.Cells[row, 4]
                            .Value?.ToString(),
                        AIFluency = sheet.Cells[row, 5]
                            .Value?.ToString(),
                        Claude101 = sheet.Cells[row, 6]
                            .Value?.ToString(),
                        OtherCertification = sheet
                            .Cells[row, 7]
                            .Value?.ToString(),
                        CoPilotTraining = sheet
                            .Cells[row, 8]
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

        public async Task<List<OnboardingStep>>
            GetOnboardingSteps()
        {
            await _lock.WaitAsync();
            try
            {
                var steps = new List<OnboardingStep>();
                using var package = new ExcelPackage(
                    new FileInfo(_filePath));
                var sheet = package.Workbook
                    .Worksheets[4];
                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    steps.Add(new OnboardingStep
                    {
                        StepID = Convert.ToInt32(
                            sheet.Cells[row, 1].Value),
                        TeamName = sheet.Cells[row, 2]
                            .Value?.ToString(),
                        StepName = sheet.Cells[row, 3]
                            .Value?.ToString(),
                        StepOrder = Convert.ToInt32(
                            sheet.Cells[row, 4].Value),
                        Description = sheet.Cells[row, 5]
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
                var sheet = package.Workbook
                    .Worksheets[5];
                for (int row = 2; row <= sheet
                    .Dimension.End.Row; row++)
                {
                    progress.Add(new OnboardingProgress
                    {
                        ProgressID = Convert.ToInt32(
                            sheet.Cells[row, 1].Value),
                        CandidateID = Convert.ToInt32(
                            sheet.Cells[row, 2].Value),
                        StepID = Convert.ToInt32(
                            sheet.Cells[row, 3].Value),
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

    }

}

               
