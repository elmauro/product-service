using AutoMapper;
using MC.ProductService.API.Data.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MC.ProductService.API.Services
{
    public abstract class ProductHandlerBase<T> : IRequestHandler<T, IActionResult> where T : IRequest<IActionResult>
    {
        protected readonly IProductRepository _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly string _internalServerErrorMessage = "Something went wrong, please try again later.";
        protected const string systemUser = "system";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductHandlerBase<typeparamref name="T"/>"/> class.
        /// </summary>
        /// <param name="repository">The product repository.</param>
        /// <param name="mapper">Automapper to map entity and model data.</param>
        /// <param name="logger">Logger for capturing runtime logs.</param>
        public ProductHandlerBase(IProductRepository repository, IMapper mapper, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ProductHandlerBase(IProductRepository repository, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Generates an ObjectResult for internal server errors.
        /// </summary>
        /// <returns>An ObjectResult configured for internal server errors.</returns>
        protected ObjectResult GetErrorObjectResult()
        {
            return new ObjectResult(_internalServerErrorMessage)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        public abstract Task<IActionResult> Handle(T request, CancellationToken cancellationToken);
    }
}
