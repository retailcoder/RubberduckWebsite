# Rubberduck.ContentServices  
Contains the logic for parsing xmldocs into website content, and for accessing the backend database.

Content services are exposed mainly via two generic interfaces:

### IContentReaderService\<TEntity>  
Exposes methods to _read_ data from the database:

 - `GetAllAsync() : Task<IEnumerable<TEntity>>`
 - `GetByIdAsync(int) : Task<TEntity>`
 - `GetByEntityKeyAsync(object) : Task<TEntity>`

The default behavior is implemented in `ContentReaderService<TEntity, TDTO>`, an abstract class from which all "reader" services should be derived.

Implementations must:

 - Supply an `IDbReaderContext` contructor argument
 - Override the `IAsyncReadRepository<TDTO> Repository` method by getting the repository from the dbcontext
 - Override `TEntity GetEntity(TDTO dto)` by providing a conversion from a DTO (get-set) type to an entity (get-only) type.
 - Override `TDTO GetDTO(TEntity entity)` by providing a conversion from a read-only entity to a read/write DTO.

### IContentWriterService\<TEntity>  
Exposes methods to _write_ data to the database:

 - `CreateAsync(TEntity) : Task<TEntity>`
 - `UpdateAsync(TEntity) : Task<TEntity>`
 - `DeleteAsync(TEntity) : Task`

The default behavior is implemented in `ContentWriterService<TEntity, TDTO>`, an abstract class from which all "writer" services should be derived.

Implementations must:

 - Supply an `IDbWriterContext` contructor argument
 - Override the `IAsyncWriteRepository<TDTO> Repository` method by getting the repository from the dbcontext
 - Override `TEntity GetEntity(TDTO dto)` by providing a conversion from a DTO (get-set) type to an entity (get-only) type.
 - Override `TDTO GetDTO(TEntity entity)` by providing a conversion from a read-only entity to a read/write DTO.

---

Other interfaces:

### IXmlDocParser  
Exposes methods to parse Rubberduck's xmldoc into `FeatureItem` objects.