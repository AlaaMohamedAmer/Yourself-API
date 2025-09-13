using yourself_demoAPI.DTOs.Home;

namespace yourself_demoAPI.Repository.Collection
{
	public interface ICollectionServices
	{
		Task<CollectionDetailsDTO> CreateCollection(CreateCollectionDTO collectionDTO);
		Task UpdateCollection(UpdateCollectionDTO collectionDTO);
		Task DeleteCollection(Guid collectionId);
		Task<CollectionDetailsDTO> GetCollection(Guid collectionId);
		Task<List<CollectionDTO>> GetAllCollections();
	}
}
