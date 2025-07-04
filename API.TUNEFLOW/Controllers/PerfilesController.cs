﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Tuneflow.Usuario.Perfiles;

namespace API.TUNEFLOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilesController : ControllerBase
    {
        /*private readonly TUNEFLOWContext _context;

        public PerfilesController(TUNEFLOWContext context)
        {
            _context = context;
        }*/

        private DbConnection connection;    

        public PerfilesController(IConfiguration config)
        {
            var connString = config.GetConnectionString("TUNEFLOWContext");
            connection = new Npgsql.NpgsqlConnection(connString);
            connection.Open();
        }

        // GET: api/Perfiles
        [HttpGet]
        public IEnumerable<Perfil> GetPerfil()
        {
            var perfiles = connection.Query<Perfil>("SELECT * FROM \"Perfiles\"");
            return perfiles;
        }

        // GET: api/Perfiles/5
        [HttpGet("{id}")]
        public ActionResult<Perfil> GetPerfil(int id)
        {
            var perfil = connection.QuerySingle<Perfil>(@"SELECT * FROM ""Perfiles"" WHERE ""Id"" = @Id", new { Id = id });

            if (perfil == null)
            {
                return NotFound();
            }

            return perfil;
        }

        // PUT: api/Perfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public void PutPerfil(int id,[FromBody] Perfil perfil)
        {
            connection.Execute(@"UPDATE ""Perfiles"" SET 
                ""ClienteId"" = @ClienteId,
                ""ArtistaId"" = @ArtistaId,
                ""ImagenPerfil"" = @ImagenPerfil,
                ""Biografia""=@Biografia,
                ""FechaCreacion""=@FechaCreacion
                
            WHERE ""Id"" = @Id", new
            {
                Id = id,
                ClienteId=perfil.ClienteId,
                ArtistaId=perfil.ArtistaId,
                ImagenPerfil = perfil.ImagenPerfil,
                Biografia = perfil.Biografia,
                FechaCreacion = perfil.FechaCreacion
            });
        }

        // POST: api/Perfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public Perfil PostPerfil([FromBody]Perfil perfil)
        {
            connection.Execute(@"INSERT INTO ""Perfiles"" (""ClienteId"", ""ArtistaId"", ""ImagenPerfil"", ""Biografia"", ""FechaCreacion"")
VALUES
            (@ClienteId, @ArtistaId, @ImagenPerfil, @Biografia, @FechaCreacion)", new
            {
                ClienteId = perfil.ClienteId,
                ArtistaId = perfil.ArtistaId,
                ImagenPerfil = perfil.ImagenPerfil,
                Biografia = perfil.Biografia,
                FechaCreacion = perfil.FechaCreacion
            });

            return perfil;
        }

        // DELETE: api/Perfiles/5
        [HttpDelete("{id}")]
        public void DeletePerfil(int id)
        {
            connection.Execute(@"DELETE FROM ""Perfiles"" WHERE ""Id"" = @Id", new { Id = id });
        }
        /*
        private bool PerfilExists(int id)
        {
            return _context.Perfiles.Any(e => e.Id == id);
        }*/
    }
}
