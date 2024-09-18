namespace Quaze.Models;


    public class Choice
    {
        public string Text { get; set; } = string.Empty;
        public bool Valid { get; set; }
        public Guid? ImageGuid { get; set; }
    }