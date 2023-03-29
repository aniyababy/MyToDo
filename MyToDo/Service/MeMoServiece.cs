using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyToDo.Shared.Dtos;

namespace MyToDo.Service
{
    public class MeMoServiece : BaseService<MeMoDto>, IMeMoService
    {
        public MeMoServiece(HttpRestClient client) : base(client, "MeMo")
        {
        }
    }
}
