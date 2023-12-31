using AutoMapper;
using DiffApplication.Domain.Actions;
using DiffApplication.Domain.Models;
using DiffApplication.Infrastructure.Repositories;
using DiffApplication.Rest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DiffApplication.Controllers
{
    /// <summary>
    /// Controller for diff actions. 
    /// </summary>
    [ApiController]
    [Route("v1/diff")]
    public class DiffController : ControllerBase
    {

        private readonly ILogger<DiffController> _logger;
        private readonly IMapper _mapper;
        private readonly IDiffRepository _diffRepository;
        private readonly IDiffResultCalculator _diffResultCalculator;


        public DiffController(ILogger<DiffController> logger, IMapper mapper, IDiffRepository diffRepository, IDiffResultCalculator diffResultCalculator) : base()
        {
            _logger = logger;
            _mapper = mapper;
            _diffRepository = diffRepository;
            _diffResultCalculator = diffResultCalculator;
        }

        // GET /v1/diff/<ID>
        /// <summary>
        /// Returns diff-ed results. 
        /// </summary>
        /// <param name="id">The ID of diff object.</param>
        /// <remarks>This endpoint is used to check diff of previously submitted data ('left' and 'right' diff object).</remarks>
        [HttpGet]
        [Route("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task <ActionResult<DiffResultViewModelGet>> GetDiffAsync(int id)
        {
            var leftDiff = await _diffRepository.GetDiffAsync(id, Const.DiffType.Left);
            var rightDiff = await _diffRepository.GetDiffAsync(id, Const.DiffType.Right);
            if (leftDiff == null || rightDiff == null)
            {
                return NotFound();
            }
            var diffResult = _diffResultCalculator.GetDiffResult(leftDiff, rightDiff);
            if (diffResult.DiffResultInfos.Count > 0)
            {
                return _mapper.Map<DiffResultWithInfoViewModelGet>(diffResult);
            }
            return _mapper.Map<DiffResultViewModelGet>(diffResult);
        }

        // PUT /v1/diff/<ID>/left
        /// <summary>
        /// Submit 'left' diff. 
        /// </summary>
        /// <param name="id">The ID of diff object.</param>
        /// <param name="diff">The diff object.</param>
        [HttpPut]
        [Route("{id}/left")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]    
        public async Task<IActionResult> PutLeftDiff(int id, DiffViewModelPut diff)
        {
            var diffDomain = _mapper.Map<Diff>(diff);
            await _diffRepository.PutDiffAsync(id, diffDomain, Const.DiffType.Left);

            diffDomain.Id = id;
            return Created("", _mapper.Map<DiffViewModelGet>(diffDomain));
        }

        // PUT /v1/diff/<ID>/right
        /// <summary>
        /// Submit 'right' diff. 
        /// </summary>
        /// <param name="id">The ID of diff object.</param>
        /// <param name="diff">The diff object.</param>
        [HttpPut]
        [Route("{id}/right")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PutRightDiff(int id, DiffViewModelPut diff)
        {
            var diffDomain = _mapper.Map<Diff>(diff);
            await _diffRepository.PutDiffAsync(id, diffDomain, Const.DiffType.Right);

            diffDomain.Id = id;
            return Created("", _mapper.Map<DiffViewModelGet>(diffDomain));
        }
    }
}
