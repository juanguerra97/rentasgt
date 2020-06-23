
namespace rentasgt.Domain.Enums
{

    /// <summary>
    /// States in which a rent request can be.
    /// Initially a request has the status Pending because it has to be 
    /// checked by the owner of the product.
    /// When the owner reads the request it has the state Viewed.
    /// If the owner accepts the request it passes to the Accepted status and if 
    /// the owner rejects it the state will be Rejected.
    /// While the request is in the Pending status it can be cancelled by 
    /// the requestor and then the status will be Cancelled
    /// </summary>
    public enum RequestStatus
    {

        Pending,
        Viewed,
        Cancelled,
        NotResolved,
        Rejected,
        Accepted

    }
}
