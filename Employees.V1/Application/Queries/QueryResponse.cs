using Newtonsoft.Json;

namespace Employees.V1.Application.Queries
{
    /// <summary>
    /// Query response
    /// </summary>
    public abstract class QueryResponsePayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryResponsePayload"/> class.
        /// </summary>
        /// <param name="maxResults">The maximum results.</param>
        /// <param name="startPosition">The start position.</param>
        protected QueryResponsePayload(int maxResults, int startPosition)
        {
            MaxResults = maxResults;
            StartPosition = startPosition;
        }

        /// <summary>
        /// Gets the maximum results.
        /// </summary>
        /// <value>
        /// The maximum results.
        /// </value>
        public int MaxResults { get; }

        /// <summary>
        /// Gets the start position.
        /// </summary>
        /// <value>
        /// The start position.
        /// </value>
        public int StartPosition { get; }
    }

    /// <summary>
    /// Query response extensions
    /// </summary>
    public static class QueryResponsePayloadExtensions
    {
        /// <summary>
        /// Converts the payload to a <see cref="QueryResponse"/>.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static QueryResponse ToQueryResponse(this QueryResponsePayload target) => new QueryResponse(target);
    }

    /// <summary>
    /// Wraps a query response
    /// </summary>
    [JsonObject]
    public class QueryResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryResponse"/> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        public QueryResponse(QueryResponsePayload payload)
        {
            QueryResponseData = payload;
        }

        /// <summary>
        /// Gets the query response data.
        /// </summary>
        /// <value>
        /// The query response data.
        /// </value>
        [JsonProperty("QueryResponse")]
        public QueryResponsePayload QueryResponseData { get; }
    }
}
