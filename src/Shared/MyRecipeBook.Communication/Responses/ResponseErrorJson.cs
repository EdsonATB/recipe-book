namespace MyRecipeBook.Communication.Responses;

public class ResponseErrorJson
{
    public List<string> Error { get; private set; }

    public ResponseErrorJson(List<string> errorMessages) => Error = errorMessages; //sintaxe de funcao oneline

    public ResponseErrorJson(string error) => Error = [error]; //caso seja exception nao conhecida (vindo do arquivo resource) ([] é criar uma lista, sintaxe curta)
}
