using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Helpers;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;

namespace PetManagementSystem.Api.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repo;
    private readonly IMapper _mapper;

    public TransactionService(ITransactionRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync(int page = 1, int pageSize = 10)
        => _mapper.Map<IEnumerable<TransactionDto>>(await _repo.GetAllAsync(page, pageSize));

    public async Task<TransactionDto> GetByIdAsync(int id)
    {
        var t = await _repo.GetByIdAsync(id);
        if (t == null)
            throw new TransactionNotFoundException(id);

        return _mapper.Map<TransactionDto>(t);
    }

    public async Task<IEnumerable<TransactionDto>> GetByCustomerAsync(int customerId, int page = 1, int pageSize = 10)
    {
        if (customerId <= 0)
            throw new ArgumentException("Customer ID must be a positive integer.", nameof(customerId));

        return _mapper.Map<IEnumerable<TransactionDto>>(await _repo.GetByCustomerAsync(customerId, page, pageSize));
    }

    public async Task<IEnumerable<TransactionDto>> GetByPetAsync(int petId)
    {
        if (petId <= 0)
            throw new ArgumentException("Pet ID must be a positive integer.", nameof(petId));

        return _mapper.Map<IEnumerable<TransactionDto>>(await _repo.GetByPetAsync(petId));
    }

    public async Task<decimal> GetTotalRevenueAsync()
        => await _repo.GetTotalRevenueAsync();

    public async Task<SalesSummaryDto> GetSalesSummaryAsync()
    {
        var all = await _repo.GetAllAsync();
        var list = all.ToList();
        return new SalesSummaryDto
        {
            TotalTransactions = list.Count,
            SuccessfulTransactions = list.Count(t => TransactionHelper.IsSuccess(t.TransactionStatus)),
            FailedTransactions = list.Count(t => TransactionHelper.IsFailed(t.TransactionStatus)),
            TotalRevenue = list.Where(t => TransactionHelper.IsSuccess(t.TransactionStatus)).Sum(t => t.Amount ?? 0)
        };
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
    {
        if (!TransactionHelper.IsValidStatus(dto.TransactionStatus))
            throw new InvalidTransactionStatusException(dto.TransactionStatus ?? "");

        var transaction = _mapper.Map<Transaction>(dto);
        var created = await _repo.CreateAsync(transaction);
        var full = await _repo.GetByIdAsync(created.TransactionId);
        return _mapper.Map<TransactionDto>(full!);
    }

    public async Task<TransactionDto> UpdateStatusAsync(int id, UpdateTransactionStatusDto dto)
    {
        if (!TransactionHelper.IsValidStatus(dto.TransactionStatus))
            throw new InvalidTransactionStatusException(dto.TransactionStatus ?? "");

        var updated = await _repo.UpdateStatusAsync(id, dto.TransactionStatus!);
        if (updated == null)
            throw new TransactionNotFoundException(id);

        return _mapper.Map<TransactionDto>(updated);
    }

    public async Task<IEnumerable<TransactionDto>> SearchAsync(string query, string? status, int page = 1, int pageSize = 10)
    {
        var results = await _repo.SearchAsync(query, status, page, pageSize);
        return _mapper.Map<IEnumerable<TransactionDto>>(results);
    }
}
