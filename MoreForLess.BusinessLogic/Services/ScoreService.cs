using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services.Interfaces;
using MoreForLess.DataAccess.EF;
using MoreForLess.DataAccess.Entities;
using NLog;

namespace MoreForLess.BusinessLogic.Services
{

    public class ScoreService : IScoreService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<ScoreDomainModel> _scoreDomainModelValidator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScoreService"/> class.
        /// </summary>
        /// <param name="context">
        ///     Context of entity framework.
        /// </param>
        /// <param name="mapper">
        ///     Instance of <see cref="Mapper"/>.
        /// </param>
        /// <param name="scoreDomainModelValidator">
        ///     Instance of type that implements interface
        ///     <see cref="IValidator{CommentDomainModel}"/>.
        /// </param>
        public ScoreService(
            ApplicationDbContext context,
            IMapper mapper,
            IValidator<ScoreDomainModel> scoreDomainModelValidator)
        {
            this._context = context;
            this._mapper = mapper;
            this._scoreDomainModelValidator = scoreDomainModelValidator;
        }

        /// <inheritdoc />
        public async Task CreateScoreAsync(ScoreDomainModel scoreDomainModel)
        {
            try
            {
                _logger.Info($"Looking for good with id: {scoreDomainModel.GoodId}");
                await this._context.Goods.SingleAsync(g => g.Id == scoreDomainModel.GoodId);
                _logger.Info($"Good with id: {scoreDomainModel.GoodId} exists in database.");
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"Good with specified id: {scoreDomainModel.GoodId} doesn't exist.", ex);
            }

            try
            {
                this._scoreDomainModelValidator.ValidateAndThrow(scoreDomainModel);
            }
            catch (ValidationException ex)
            {
                throw new ArgumentException($"Error when validating {scoreDomainModel}.", ex);
            }

            var score = this._mapper.Map<Score>(scoreDomainModel);

            _logger.Info("Adding score to database");
            this._context.Scores.Add(score);


            _logger.Info("Saving changes to database");
            await this._context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<double> GetAverageScoreAsync(int goodId)
        {
            _logger.Info("Getting scores from database.");
            var scores = await this._context.Scores.Where(c => c.GoodId == goodId).ToListAsync();

            if (scores.Count == 0)
            {
                throw new ArgumentException($"Scores for good with id: {goodId} haven't been added yet.");
            }

            return scores.Average(s => s.Value);
        }

        /// <inheritdoc />
        public async Task DeleteScoresAsync(int goodId)
        {
            try
            {
                _logger.Info($"Looking for good with id: {goodId}");
                await this._context.Goods.SingleAsync(g => g.Id == goodId);
                _logger.Info($"Good with id: {goodId} exists in database.");
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"Good with specified id: {goodId} doesn't exist.", ex);
            }

            _logger.Info($"Getting all scores of good with id: {goodId}.");
            var scores = await this._context.Scores.Where(c => c.GoodId == goodId).ToListAsync();

            if (scores.Count == 0)
            {
                throw new ArgumentException($"Scores for good with id: {goodId} haven't been added yet.");
            }

            _logger.Info($"Deleting all scores of good with id: {goodId}.");
            this._context.Scores.RemoveRange(scores);

            _logger.Info("Saving changes to database");
            await this._context.SaveChangesAsync();
        }
    }
}
