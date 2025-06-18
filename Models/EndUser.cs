namespace CareerBuilderX.Models
{
    public class EndUser:Person
    {
        public List<Resume> Resumes {  get; set; }

        public List<Portfolio> Portfolios { get; set; }

    }
}
