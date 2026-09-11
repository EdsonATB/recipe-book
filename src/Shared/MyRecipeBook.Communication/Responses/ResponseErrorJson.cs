namespace MyRecipeBook.Communication.Responses;

public class ResponseErrorJson
{
    public List<string> Errors { get; private set; }

    public ResponseErrorJson(List<string> errorMessages) => Errors = errorMessages; //sintaxe de funcao oneline

    public ResponseErrorJson(string error) => Errors = [error]; //caso seja exception nao conhecida (vindo do arquivo resource) ([] é criar uma lista, sintaxe curta)
}
