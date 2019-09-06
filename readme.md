# API Demo

## API Versions
>There are two versions of the API. These can be selected via the swagger dropdown.
### Version 1 (V1)
> This matches the provided [specification](spec.md).

The `maxResults` and `startPosition` are hard-coded to as I have no context as to the purpose of those properties in retrieving a singular item as opposed to a search. If a matching record is found the values will be 1 for both properties.

### Version 2 (V2)
> Assuming we have the capability of dictating the API surface to third-party consumers, this is how I would have suggested it be structured.
- JSON keys are all camel-cased rather than an inconsistent mix of Pascal and camel-casing
- Redundant properties and nesting are removed. It seems like this response was a mix of a paged list (`maxResults`, `startPosition`) mixed with a singular item (`Employee`). 
- Route is changed to be more RESTful as it looks like the aim is to retrieve the _Detail_ resource of an _Employee_

## Persistence
I used [JsonFlatFileDataStore](https://github.com/ttu/json-flatfile-datastore) as it did the job for this demo (not production ready). 

## Logging
> Logging to file and console via Serilog

The majority of the API plumbing logging (urls, response code, timing) is handled by the _Request logging_ feature of [Serilog.AspNetCore package](https://github.com/serilog/serilog-aspnetcore), which negates the need for any additional logging in the controllers as they are just a wrapper around their respective query services.

Most of the logging added is trace level which is filtered out in the current Serilog configuration.

## Testing
[![Coverage Stats](test/artifacts/badge_combined.svg)](test/artifacts/Summary.mht)

> We weren't supplied with any BDD artifacts, hence SpecFlow wasn't used.
### Unit Tests
> Unit tests use NUnit.

Each API version has it's own test project as the framework.
### Integration Tests
> Integration tests use xUnit.

Integration tests are combined in a single project as it has a dependency on the api host project and thus includes both versions. 

