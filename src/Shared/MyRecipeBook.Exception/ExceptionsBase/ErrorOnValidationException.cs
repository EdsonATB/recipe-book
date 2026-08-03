namespace MyRecipeBook.Exception.ExceptionsBase;

public class ErrorOnValidationException : MyRecipeBookException
{
    private readonly List<string> _errors; //recomendacao da microsoft comecar atributos que forem private readonly com _ 
    
    public ErrorOnValidationException(List<string> errorMessages) => _errors = errorMessages;


    public List<string> getErrorMessages() => _errors; //return
}
