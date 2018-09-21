namespace MaaltijdApplicatie.Models.Domain {

    public class DomainMethodResult {

        public bool WasSuccessful { get; set; } = false;

        public string Message { get; set; }

        public void Succeeded() =>
            WasSuccessful = true;

    }

}
