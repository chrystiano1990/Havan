using Dapper;
using Havan.Application.Models;
using Havan.Application.ModelsIn;
using Havan.Domain.Database;
using Havan.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Havan.Services
{
	public class HavanService
	{
		private SqlConnection _Dapper;
		private IConfiguration _configuration;
		public HavanService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<dynamic> CriaTicket(AddTicket dados)
		{
			using (var db = new Connection(_configuration.GetConnectionString("Havan")))
			using (var trans = db.Database.BeginTransaction())
			{
				try
				{
					var validaTicket = await db.Ticket.FirstOrDefaultAsync(x => x.IdCliente == dados.IdCliente && x.IdSituacao == 1);

					if (validaTicket != null) throw new Exception("Cliente já possui um ticket em andamento");

					await db.AddAsync(new Ticket
					{
						IdSituacao = 1,
						DataAbertura = DateTime.Now,
						Codigo = dados.Codigo,
						IdCliente = dados.IdCliente,
						IdUsuarioAbertura = dados.IdUsuarioAbertura
					});
					await db.SaveChangesAsync();
					trans.Commit();

					return "Ticket criado com sucesso";
				}
				catch (Exception ex)
				{
					trans.Rollback();
					throw ex;
				}

			}

		}

		public async Task<dynamic> AnotaTicket(AnotaTicket dados)
		{
			using (var db = new Connection(_configuration.GetConnectionString("Havan")))
			using (var trans = db.Database.BeginTransaction())
			{
				try
				{
					await db.AddAsync(new TicketAnotacao
					{
						IdTicket = dados.IdTicket,
						IdUsuario = dados.IdUsuario,
						Texto = dados.Texto,
						Data = DateTime.Now
					});
					await db.SaveChangesAsync();
					trans.Commit();

					return "Anotações finalizadas";
				}
				catch (Exception ex)
				{
					trans.Rollback();
					throw ex;
				}
			}
		}


		public async Task<dynamic> ConcluiTicket(FechaTicket dados)
		{
			using (var db = new Connection(_configuration.GetConnectionString("Havan")))
			using (var trans = db.Database.BeginTransaction())
			{
				try
				{
					var ticket = await db.Ticket.FirstOrDefaultAsync(x => x.Id == dados.TicketId && x.IdSituacao == 1);
					if (ticket == null) throw new Exception("Ticket não existe ou já foi concluído");

					ticket.DataConclusoa = DateTime.Now;
					ticket.IdUsuarioConclusao = dados.IdUsuarioConclusao;
					ticket.IdSituacao = 2;

					await db.SaveChangesAsync();
					trans.Commit();

					return "Ticket concluído";
				}
				catch (Exception ex)
				{
					trans.Rollback();
					throw ex;
				}

			}
		}

		public async Task<dynamic> ListarTickets(Int64 TicketId)
		{
			using (var _Dapper = new SqlConnection(_configuration.GetConnectionString("Havan")))
			{
				try
				{
					_Dapper.Open();

					var RetornoProc = _Dapper.Query<dynamic>("sp_ListaTicket", new
					{
						@TicketId = TicketId
					}, commandType: CommandType.StoredProcedure, commandTimeout: 0).FirstOrDefault();

					_Dapper.Close();


					return RetornoProc;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}

		}

		public async Task<dynamic> Login(string Usuario, string Senha)
		{
			using (var db = new Connection(_configuration.GetConnectionString("Havan")))
			{
				try
				{
					var usuario = await db.Usuario.FirstOrDefaultAsync(x => x.Nome.ToLower() == Usuario.ToLower() && x.Codigo == Senha);
					if (usuario == null) throw new Exception("Usuário ou senha inválidos");

					var token = GerarToken(Usuario, Senha);

					return new 
					{
						Usuario = usuario.Nome,
						Token = token
					};

				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		private async Task<dynamic> GerarToken(string Usuario, string Senha)
		{
			using (var db = new Connection(_configuration.GetConnectionString("Havan")))
			{
				try
				{
					var tokenHandler = new JwtSecurityTokenHandler();

					byte[] key = Encoding.ASCII.GetBytes(ApiSettings.Secret);

					var tokenDescripto = new SecurityTokenDescriptor
					{
						Subject = new ClaimsIdentity(new[]
						{
							new Claim(type:ClaimTypes.Name, value:Usuario)
						}),

						Expires = DateTime.Now.AddHours(4),
						SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), algorithm: SecurityAlgorithms.HmacSha256Signature)
					};

					var token = tokenHandler.CreateToken(tokenDescripto);

					return tokenHandler.WriteToken(token);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}
	}
}
