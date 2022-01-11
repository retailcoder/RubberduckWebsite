namespace Rubberduck.Model.Abstract
{
    public interface IInternalModel<TPublicModel>
    {
        TPublicModel ToPublicModel();
    }
}
