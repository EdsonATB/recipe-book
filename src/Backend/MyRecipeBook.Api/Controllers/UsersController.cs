using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost] //define que isso é um endpoint
    public IActionResult Register([FromBody]RequestRegisterUserAccountJson request) 
    {
        var useCase = new RegisterUserAccountUseCase();
        useCase.Execute(request);
        return Created(); //devolve resposta 201 (created) pra quem solicitou esse endpoint
    }
}
