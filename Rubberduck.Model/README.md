# Rubberduck.Model  
Defines .NET types that map to the tables of the backing database. This project may not reference other projects in the solution.

## Model.DTO  
This namespace defines simple read/write objects derived from the `BaseDto` abstract class, mapped to a specific table with a `[Table]` attribute.

DTO types are implemented by exposing `{ get; set; }` auto-properties for each column of the mapped table.

## Model.Entity  
This namespace defines immutable objects that can round-trip to/from a DTO type. These classes implement the `IEntity` interface.

Each entity must expose a `Key` method that returns an anonymous object with the properties that make up a unique _natural key_ that `GetByKey` methods use. For example `Feature.Name` is our unique key for the `Feature` entity:

```csharp
public object Key() => new { Name };
```

---

## Conventions  
Entity types have a `private` constructor and expose `public static` methods for construction:

 - `FromDTO(TDTO) : TEntity`
 - `ToDTO(TEntity) : TDTO`

These methods may be overloaded as convenient.

Repositories use DTO types as generic type constraints and return read/write objects to a service layer that converts them to read-only entities for processing; the UI receives read/write DTO objects.

ViewModel types may have a `public` default/parameterless constructor, and expose `{ get; set; }` auto-properties.