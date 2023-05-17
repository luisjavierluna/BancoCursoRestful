using Application.DTOs;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clientes.Queries.GetAllClientes
{
    public class GetAllClientesQuery : IRequest<PagedResponse<List<ClienteDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public class GetAllClientesQueryHandler : IRequestHandler<GetAllClientesQuery, PagedResponse<List<ClienteDto>>>
        {
            private readonly IRepositoryAsync<Cliente> _repositoryAsync;
            private readonly IMapper _mapper;

            public GetAllClientesQueryHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
            {
                _repositoryAsync = repositoryAsync;
                _mapper = mapper;
            }

            public async Task<PagedResponse<List<ClienteDto>>> Handle(GetAllClientesQuery request, CancellationToken cancellationToken)
            {
                var clientes = await _repositoryAsync.ListAsync(new PagedClientesSpecification(request.PageSize, request.PageNumber, request.Nombre, request.Apellido));
                var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);

                return new PagedResponse<List<ClienteDto>>(clientesDto, request.PageNumber, request.PageSize);
            }
        }
    }
}
