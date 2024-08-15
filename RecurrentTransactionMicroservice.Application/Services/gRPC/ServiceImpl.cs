using AutoMapper;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using RecurrentTransactionMicroservice.Domain.Models;
using RecurrentTransactionMicroservice.Persistence.Context;

namespace RecurrentTransactionMicroservice.Core.Services.gRPC;

public class ServiceImpl : RecurrentTransactionService.RecurrentTransactionServiceBase
{
    private readonly IMapper _mapper;
    private readonly RecurrentTransactionsDbContext _context;

    public ServiceImpl(IMapper mapper, RecurrentTransactionsDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public override async Task<CreateRecurrentTransactionResponse> CreateRecurrentTransaction(CreateRecurrentTransactionRequest request, ServerCallContext context)
    {
        
        try
        {
            var recurrentTransaction = _mapper.Map<RecurrentTransaction>(request);
            _context.RecurrentTransactions.Add(recurrentTransaction);
            await _context.SaveChangesAsync();
            var response = new CreateRecurrentTransactionResponse
            {
                Transaction = _mapper.Map<RecurrentTransactions>(recurrentTransaction)
            };

            return response;
        }
        catch (Exception ex)
        {
            // Log the error
            throw new RpcException(new Status(StatusCode.Internal, $"An error occurred while creating the transaction: {ex.Message}"));
        }
    }

    public override async Task<GetRecurrentTransactionResponse> GetRecurrentTransaction(GetRecurrentTransactionRequest request, ServerCallContext context)
    {
        try
        {
            
            var transaction = await _context.RecurrentTransactions.FindAsync(request.Id);
            if (transaction == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Transaction not found"));
            }

            var response = new GetRecurrentTransactionResponse
            {
                Transaction = _mapper.Map<RecurrentTransactions>(transaction)
            };

            return response;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, $"An error occurred while retrieving the transaction: {ex.Message}"));
        }
    }


    public override async Task<ListRecurrentTransactionsResponse> ListRecurrentTransactions(ListRecurrentTransactionsRequest request, ServerCallContext context)
    {
        try
        {
            var transactions = await _context.RecurrentTransactions.ToListAsync();
            var response = new ListRecurrentTransactionsResponse();
            response.Transactions.AddRange(_mapper.Map<IEnumerable<RecurrentTransactions>>(transactions));

            return response;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, $"An error occurred while listing transactions: {ex.Message}"));
        }
    }

}