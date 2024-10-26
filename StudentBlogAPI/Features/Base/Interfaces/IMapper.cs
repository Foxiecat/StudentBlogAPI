namespace StudentBlogAPI.Features.Base.Interfaces;

public interface IMapper<TModel, TDTO>
{
    TDTO MapToDTO(TModel model);
    TModel MapToModel(TDTO dto);
}