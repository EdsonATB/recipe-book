using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost] //define que isso é um endpoint
    public IActionResult Register([FromBody]RequestRegisterUserAccountJson request, [FromServices] IRegisterUserAccountUseCase useCase) 
    {
        useCase.Execute(request);
        return Created(); //devolve resposta 201 (created) pra quem solicitou esse endpoint
    }
}
