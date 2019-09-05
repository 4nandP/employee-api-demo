# API Demo
[![Coverage Stats](test/artifacts/badge_combined.svg)](test/artifacts/summary.mht)

## API Versions
There are two versions of the API. These can be selected via the swagger dropdown.
### Version 1 (V1)
> This matches the provided [specification](spec.md)

### Version 2 (V2)
> Assuming we have the capability of dictating the API surface to third-party consumers, this is how I would have structured it.
- JSON keys are all camel-cased rather than an inconsistent mix of Pascal and camel-casing
- Redundant properties and nesting are removed. It seems like this response was a mix of a paged list (`maxResults`, `startPosition`) mixed with a singular item (`Employee`). 
- Route is changed to be more RESTful as it looks like the aim is to retrieve the _Details_ resource of an _Employee_