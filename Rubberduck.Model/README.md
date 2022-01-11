# Rubberduck.Model  
Defines .NET types that map to the tables of the backing database, among other DTOs. This project may not reference other projects in the solution.

## Model.Abstract  
This namespace defines the `Entity` abstract class from which database-mapped classes are derived, and other required abstractions, such as `IIndenterSettings`, which `RubberduckServices` implements in an adapter that removes the need for other projects to directly reference any Rubberduck assemblies.


## Model.Entities  
This namespace defines classes derived from `Entity`, inheriting the `IEntity` interface representing a type that is mapped to a database table.

Non-entity model classes should be in the parent `Rubberduck.Model` namespace.