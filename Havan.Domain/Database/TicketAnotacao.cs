namespace Havan.Domain.Database
{
	public class TicketAnotacao
	{
		public Int64 Id { get; set; }
		public Int64? IdTicket { get; set; }
		public Int64? IdUsuario { get; set; }
		public string? Texto { get; set; }
		public DateTime? Data { get; set; }
	}
}
