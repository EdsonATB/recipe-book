using Konscious.Security.Cryptography;
using MyRecipeBook.Domain.Security.PasswordHashing;
using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.PasswordHashing;

internal sealed class Argon2PasswordHasher : IPasswordHasher //sealed significa bloquear que outras classes usem essa aqui como herança
{
    private const int DEGREE_OF_PARALLELISM = 1;
    private const int ITERATIONS = 2;
    private const int MEMORY_SIZE = 20 * 1024;
    private const int SALT_SIZE = 16;
    private const int HASH_SIZE = 32;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SALT_SIZE); //gera bytes a partir de um numero aleatorio com o tamanho definido na constante

        var hash = HashAlgorithm(password, salt); //executa a funcao privada que contem o algoritmo para encryptar. Devolve o hash em bytes

        //GetBytes() devolve o valor convertido em bytes, depois usamos o length para determinar o tamanho desses bytes
        var combinedBytes = new byte[salt.Length + hash.Length]; //aqui teremos o salt + o hash. Para podermos verificar a senha depois

        salt.CopyTo(combinedBytes); //copia o salt pro array de bytes
        hash.CopyTo(combinedBytes, index: salt.Length); //copia o hash a partir do final do salt para nao sobreescrever (fazemos isso pois precisamos do hash e do salt para verificar a senha)

        return Convert.ToBase64String(combinedBytes); //converte pra string e devolve
    }

    public bool VerifyPassword(string password, string hashedPassword) //senha inputada e senha do banco ja hashada
    {
        var combinedBytes = Convert.FromBase64String(hashedPassword);
        var salt = new byte[SALT_SIZE];
        var hash = new byte[HASH_SIZE];

        Array.Copy(combinedBytes, salt, SALT_SIZE);
        Array.Copy(combinedBytes, SALT_SIZE, hash, 0,HASH_SIZE);

        var newHash = HashAlgorithm(password, salt);

        return CryptographicOperations.FixedTimeEquals(newHash, hash);
    }

    private byte[] HashAlgorithm(string password, byte[] salt) {

        var passwordBytes = Encoding.UTF8.GetBytes(password); //converte a senha em bytes

        //define as configs do algoritmo usando as constantes definidas
        var hashAlgorithm = new Argon2id(passwordBytes)
        {
            DegreeOfParallelism = DEGREE_OF_PARALLELISM,
            Iterations = ITERATIONS,
            MemorySize = MEMORY_SIZE,
            Salt = salt
        };

        return hashAlgorithm.GetBytes(HASH_SIZE);
    }
}
