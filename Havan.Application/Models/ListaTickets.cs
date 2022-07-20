namespace Havan.Application.Models
{
	public class ListaTickets
	{
		public Int64 Id { get; set; }
		public Int64? IdUsuarioAbertura { get; set; }
		public Int64? IdUsuarioConclusao { get; set; }
		public Int64? IdCliente { get; set; }
		public Int16? IdSituacao { get; set; }
		public int? Codigo { get; set; }
		public DateTime? DataAbertura { get; set; }
		public DateTime? DataConclusoa { get; set; }
		public string? CodigoCliente { get; set; }
		public string? CPF { get; set; }
		public string? Status { get; set; }
		public string? CodigoUsuario { get; set; }
		public string? Nome { get; set; }
	}
}
