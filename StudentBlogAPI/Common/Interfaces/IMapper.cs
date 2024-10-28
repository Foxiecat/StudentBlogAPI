namespace StudentBlogAPI.Features.Common.Interfaces;

public interface IMapper<TModel, TDTO>
{
    TDTO MapToDTO(TModel model);
    TModel MapToModel(TDTO dto);
}