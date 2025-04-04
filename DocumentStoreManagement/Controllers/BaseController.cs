using Microsoft.AspNetCore.Mvc;

namespace DocumentStoreManagement.Controllers
{
    /// <summary>
    /// Base controller for databases which need UnitOfWork
    /// </summary>
    /// <remarks>
    /// Constructor for unit of work
    /// </remarks>
    public class BaseController : ControllerBase
    {
    }
}
