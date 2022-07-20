using Havan.Application.ModelsIn;
using Havan.Services;
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

		[HttpPost("CriaTicket")]
		public async Task<dynamic> CriaTicket(AddTicket dados)
		{
			return await _service.CriaTicket(dados);
		}

		[HttpPost("AnotaTicket")]
		public async Task<dynamic> AnotaTicket(AnotaTicket dados)
		{
			return await _service.AnotaTicket(dados);
		}

		[HttpPut("ConcluiTicket")]
		public async Task<dynamic> ConcluiTicket(FechaTicket dados)
		{
			return await _service.ConcluiTicket(dados);
		}

		[HttpGet("ListarTicket")]
		public async Task<dynamic> ListarTicket(Int64 TicketId)
		{
			return await _service.ListarTickets(TicketId);
		}

	}
}