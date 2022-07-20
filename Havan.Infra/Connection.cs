using Havan.Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace Havan.Infra
{
	public class Connection : DbContext
	{

		private string _cs;
		public Connection(string cs) : base()
		{
			_cs = cs;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_cs);
		}


		public DbSet<Cliente> Cliente { get; set; }
		public DbSet<Ticket> Ticket { get; set; }
		public DbSet<TicketAnotacao> TicketAnotacao { get; set; }
		public DbSet<TicketSituacao> TicketSituacao { get; set; }
		public DbSet<Usuario> Usuario { get; set; }

	}
}