namespace MyRecipeBook.Communication.Responses;

public class ResponseErrorJson //é usado para organizar o retorno da API, é usado com new entao no retorno vai ter as propriedades dessa classe organizadas, caso retornasse o getErrors de outra classe de exceçao da camada de exception so viria uma lista desorganizada e "sem possibilidade" de adicionar novas propriedades.
{
    public List<string> Errors { get; private set; }

    public ResponseErrorJson(List<string> errorMessages) => Errors = errorMessages; //sintaxe de funcao oneline

    public ResponseErrorJson(string error) => Errors = [error]; //caso seja exception nao conhecida (vindo do arquivo resource) ([] é criar uma lista, sintaxe curta)
}
