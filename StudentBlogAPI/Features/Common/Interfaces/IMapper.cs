namespace StudentBlogAPI.Features.Common.Interfaces;

public interface IMapper<TModel, TDTO>
{
    TDTO MapToResponse(TModel model);
    TModel MapToModel(TDTO dto);
}