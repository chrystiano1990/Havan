using Havan.Application.ModelsIn;
using Havan.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Havan.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HavanController : ControllerBase
	{
		private readonly HavanService _service;

		public HavanController(HavanService havanService)
		{
			_service = havanService;
		}

		[HttpPost("CriaTicket"), Authorize]
		public async Task<dynamic> CriaTicket(AddTicket dados)
		{
			return await _service.CriaTicket(dados);
		}

		[HttpPost("AnotaTicket"), Authorize]
		public async Task<dynamic> AnotaTicket(AnotaTicket dados)
		{
			return await _service.AnotaTicket(dados);
		}

		[HttpPut("ConcluiTicket"), Authorize]
		public async Task<dynamic> ConcluiTicket(FechaTicket dados)
		{
			return await _service.ConcluiTicket(dados);
		}

		[HttpGet("ListarTicket"), Authorize]
		public async Task<dynamic> ListarTicket(Int64 TicketId)
		{
			return await _service.ListarTickets(TicketId);
		}

		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<dynamic> Login(string Usuario, string Senha)
		{
			return await _service.Login(Usuario, Senha);
		}

	}
}