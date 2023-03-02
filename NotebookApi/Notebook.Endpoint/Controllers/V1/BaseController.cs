using Microsoft.AspNetCore.Mvc;
using Notebook.DataService.IConfiguration;

namespace Notebook.Endpoint.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{
    protected IUnitOfWork _unitOfWork;

    public BaseController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}