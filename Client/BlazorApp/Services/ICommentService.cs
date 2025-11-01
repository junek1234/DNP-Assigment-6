using System;
using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    public Task UpdateCommentAsync(int id, UpdateCommentDto request);
    public Task DeleteCommentAsync(int id);
    public Task<CommentDto?> GetCommentAsync(int id);
    public Task<List<CommentDto>> GetComments();
    public Task<List<CommentDto>> GetCommentsByPostId(int postId);
}
