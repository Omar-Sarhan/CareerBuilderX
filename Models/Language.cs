namespace CareerBuilderX.Models
{
    public class Language
    {
        public int LanguageId { get; set; }

        public string LanguageName{ get; set; }

        public int Level { get; set; }

        public Resume Resume { get; set; }

        public int ResumeId { get; set; }
    }
}
